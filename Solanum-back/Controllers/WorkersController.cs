using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Solanum_back.Models;

namespace Solanum_back.Controllers {

    [Route("solanum/[controller]")]
    [Authorize]
    [ApiController]
    public class WorkersController : ControllerBase {

        private readonly SolanumContext _context;

        public WorkersController(SolanumContext context) { 
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkers([FromQuery] int bossID) {
            List<Trabajadore> workers = _context.Trabajadores.Where(w => w.Contratos.Any(c => c.IdUsuario == bossID) && w.Estado == "A").ToList();
            if (workers != null) {
                return Ok(workers);
            } else {
                return BadRequest("Usuario no encontrado");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerInfo workerInfo) {
            if (!WorkerExists(workerInfo)) {
                Trabajadore worker = new Trabajadore() {
                    NombreTrabajador = workerInfo.Name,
                    NumeroContacto = workerInfo.ContactNumber,
                    Estado = "A"
                };
                Trabajadore createdWorker = _context.Trabajadores.Add(worker).Entity;
                await _context.SaveChangesAsync();
                Usuario boss = _context.Usuarios.Where(b => b.IdUsuario == workerInfo.BossID).FirstOrDefault();
                Contrato contract = new Contrato() {
                    IdTrabajador = createdWorker.IdTrabajador,
                    IdUsuario = boss.IdUsuario,
                    Estado = "A"
                };
                _context.Contratos.Add(contract);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Trabajador agregado con exito" });
            } else {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Ya existe el trabajador" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditWorker([FromBody] EditWorkerInfo info) {
            Trabajadore worker = _context.Trabajadores.Where(w => w.IdTrabajador == info.Id).FirstOrDefault();
            if (worker != null) {
                worker.NombreTrabajador = info.Name is null ? worker.NombreTrabajador : info.Name;
                worker.NumeroContacto = info.ContactNumber is null ? worker.NumeroContacto : info.ContactNumber;
                _context.Trabajadores.Update(worker);
                await _context.SaveChangesAsync();
                return Ok("Trabajador actualizado con exito");
            } else {
                return NotFound("Trabajador no encontrado");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> DeleteWorker([FromQuery] int workerID, [FromQuery] int bossID) {
            Trabajadore worker = _context.Trabajadores.Where(w => w.IdTrabajador == workerID).FirstOrDefault();
            if (worker != null) {
                worker.Estado = "D";
                Contrato contract = _context.Contratos.Where(c => c.IdTrabajador == workerID && c.IdUsuario == bossID).FirstOrDefault();
                contract.Estado = "D";
                _context.Trabajadores.Update(worker);
                await _context.SaveChangesAsync();
                return Ok("Trabajador eliminado con exito");
            } else {
                return NotFound("Trabajador no encontrado");
            }
        }

        private bool WorkerExists(WorkerInfo info) {
            return (_context.Trabajadores?.Any(e => e.NombreTrabajador == info.Name)).GetValueOrDefault();
        }
    }
}
