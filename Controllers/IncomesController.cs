using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Solanum_back.Models;

namespace Solanum_back.Controllers
{

    [Route("solanum/[controller]")]
    [Authorize]
    [ApiController]

    public class IncomesController : ControllerBase

    {
        private readonly SolanumContext _context;

        public IncomesController(SolanumContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncomes([FromQuery] int IDCrop)
        { 
            List<Ingreso> ingresos = _context.Ingresos.Where(u => u.IdCultivo == IDCrop && u.Estado == "A").ToList<Ingreso>();
            double value = ingresos.Sum(e => e.ValorIngreso);
            return Ok(new { incomes = ingresos, total = value });
        }

        [HttpPost]
        public async Task<IActionResult> createIncome([FromBody] IncomeInfo info)
        {
            if (!IncomeExists(info))
            {
                Ingreso ingreso = new Ingreso()
                {
                    NombreIngreso = info.name,
                    FechaIngreso = info.date,
                    ValorIngreso = info.value,
                    DescripcionIngreso = info.description,
                    IdCultivo = info.cropId,
                    Estado = "A"
                };

                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ingreso agregado con exito" });
            }
            else {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Ya existe el ingreso" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> editIncome([FromBody] EditIncomeInfo info)
        {
            Ingreso ingreso = _context.Ingresos.Where(i => i.IdIngreso == info.incomeId).FirstOrDefault();
            if (ingreso == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Ingreso no encontrado");
            }
            else {
                ingreso.NombreIngreso = info.name is null ? ingreso.NombreIngreso : info.name;
                ingreso.FechaIngreso = info.date;
                ingreso.ValorIngreso = info.value is 0 ? ingreso.ValorIngreso : info.value;
                ingreso.Cantidad = info.quantity is 0 ? ingreso.Cantidad : info.quantity;
                ingreso.DescripcionIngreso = info.description is null ? ingreso.DescripcionIngreso : info.description;
                _context.Ingresos.Update(ingreso);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "ingreso actualizado con exito");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> deleteIncome([FromQuery] int incomeID)
        {
            Ingreso ingreso = _context.Ingresos.Where(i => i.IdIngreso == incomeID).FirstOrDefault();
            if (ingreso == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Ingreso no encontrado");
            }
            else
            {
                ingreso.Estado = "D";
                _context.Ingresos.Update(ingreso);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "ingreso eliminado con exito");
            }
        }



        private bool IncomeExists(IncomeInfo info)
        {
            return (_context.Ingresos?.Any(e => e.NombreIngreso == info.name)).GetValueOrDefault();
        }
    }
}