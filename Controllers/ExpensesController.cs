using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solanum_back.Models;

namespace Solanum_back.Controllers {
    [Route("solanum/[controller]")]
    [Authorize]
    [ApiController]
    public class ExpensesController : ControllerBase {

        private readonly SolanumContext _context;

        public ExpensesController(SolanumContext context) {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] int cropID, [FromQuery] string stage, [FromQuery] string type) {
            List<Gasto> expenses = _context.Gastos.Where(g => g.IdCultivo == cropID && g.Etapa == stage && g.TipoGasto == type && g.Estado == "A").ToList();
            double value = expenses.Sum(e => e.ValorGasto);
            double totalValue = _context.Gastos.Where(g => g.IdCultivo == cropID && g.Etapa == stage && g.Estado == "A").Sum(s => s.ValorGasto);
            if (type == "MO") {
                List<LaborExpenseInfo> laborExpenses = new List<LaborExpenseInfo>();
                foreach (Gasto gasto in expenses) {
                    LaborExpenseInfo laborExpense = new LaborExpenseInfo() {
                        IdGasto = gasto.IdGasto,
                        NombreGasto = gasto.NombreGasto,
                        TipoGasto = gasto.TipoGasto,
                        ValorGasto = gasto.ValorGasto,
                        DescripcionGasto = gasto.DescripcionGasto,
                        Etapa = gasto.Etapa,
                        IdCultivo = gasto.IdCultivo,
                        IdTrabajador = gasto.IdTrabajador,
                        Estado = gasto.Estado,
                        EstadoPago = gasto.EstadoPago,
                        NumeroJorbul = gasto.NumeroJorbul,
                        CalorUnidad = gasto.CalorUnidad,
                        FechaGasto = gasto.FechaGasto,
                        workerName = searchWorkerByID(gasto.IdTrabajador).NombreTrabajador
                    };
                    laborExpenses.Add(laborExpense);
                }
                return Ok(new { expenses = laborExpenses, total = totalValue });
            }
            return Ok(new {expenses = expenses, total = totalValue });
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseInfo expenseInfo) {
            Gasto expense = new Gasto() {
                NombreGasto = expenseInfo.Name,
                TipoGasto = expenseInfo.type,
                ValorGasto = expenseInfo.value,
                DescripcionGasto = expenseInfo.description,
                Etapa = expenseInfo.stage,
                IdCultivo = expenseInfo.cropID,
                Estado = "A",
                EstadoPago = expenseInfo.paidState,
                FechaGasto = expenseInfo.date,
                NumeroJorbul = expenseInfo.jorbulNumber,
                CalorUnidad = expenseInfo.unitValue,
            };
            if (expenseInfo.type == "MO") {
                Trabajadore worker = searchWorker(expenseInfo.workerName);
                if (worker != null) {
                    expense.IdTrabajador = worker.IdTrabajador;
                } else {
                    return NotFound(new { message = "No existe un trabajador con ese nombre" });
                }
            }
            _context.Gastos.Add(expense);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Gasto agregado con exito" });
        }

        [HttpPut]
        public async Task<IActionResult> EditExpense([FromBody] EditExpenseInfo info) {
            Gasto expense = _context.Gastos.Where(e => e.IdGasto == info.id).FirstOrDefault();
            if (expense != null) {
                expense.NombreGasto = info.name is null ? expense.NombreGasto : info.name;
                expense.DescripcionGasto = info.description is null ? expense.DescripcionGasto : info.description;
                expense.EstadoPago = info.paidState is null ? expense.EstadoPago : info.paidState;
                expense.FechaGasto = info.date;
                expense.CalorUnidad = info.unitValue;
                expense.ValorGasto = info.value;
                expense.NumeroJorbul = info.jorbulNumber;
                if (info.type == "MO") {
                    Trabajadore worker = searchWorker(info.workerName);
                    if (worker != null) {
                        expense.IdTrabajador = worker.IdTrabajador;
                    } else {
                        return NotFound(new { message = "No existe un trabajador con ese nombre" });
                    }
                }
                _context.Gastos.Update(expense);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Gasto actualizado con exito" });
            } else {
                return NotFound(new {message = "Gasto no encontrado" });
            }
        } 

        [HttpPatch]
        public async Task<IActionResult> DeleteExpense([FromQuery] int expenseID) {
            Gasto expense = _context.Gastos.Where(e => e.IdGasto == expenseID).FirstOrDefault();
            if (expense is null) {
                return BadRequest("Gasto no encontrado");
            } else {
                expense.Estado = "D";
                _context.Gastos.Update(expense);
                await _context.SaveChangesAsync();
                return Ok(new {message = "Gasto eliminado con exito"});
            }
        }

        private Trabajadore searchWorker(string workerName) {
            return _context.Trabajadores.Where(w => w.NombreTrabajador.ToLower() == workerName.ToLower()).FirstOrDefault();
        }

        private Trabajadore searchWorkerByID(int? workerID) {
            return _context.Trabajadores.Where(w => w.IdTrabajador == workerID).FirstOrDefault();
        }
    }
}
