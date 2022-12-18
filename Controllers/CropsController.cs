using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solanum_back.Models;

namespace Solanum_back.Controllers {

    [Route("solanum/[controller]")]
    [Authorize]
    [ApiController]
    public class CropsController : ControllerBase {

        private readonly SolanumContext _context;

        public CropsController(SolanumContext context) { 
            this._context = context;
        }

        [HttpGet]
        [Route("Reports")]
        public async Task<IActionResult> GetCropsReport([FromQuery] int userID) {
            if (UserExists(userID)) {
                List<Cultivo> cultivos = _context.Cultivos.Where(u => u.IdUsuario == userID && u.Estado == "A").ToList<Cultivo>();
                List<CropReport> reports = GenerateCropsReports(cultivos);
                int totalIncomes = 0;
                int totalExpenses = 0;
                foreach (CropReport report in reports) {
                    totalIncomes += report.totalIncomes;
                    totalExpenses += report.totalExpenses;
                }
                return Ok(new {reports = reports , generalReport = new { incomes = totalIncomes, expenses = totalExpenses} });
            } else {
                return BadRequest(new { message = "Usuario no encontrado" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCrops([FromQuery] int userID) {
            if (UserExists(userID)) {
                List<Cultivo> cultivos = _context.Cultivos.Where(u => u.IdUsuario == userID && u.Estado == "A").ToList<Cultivo>();
                return Ok(cultivos);
            } else {
                return BadRequest(new {message = "Usuario no encontrado" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCrop([FromBody] CropInfo cropInfo) {
            if (!CropExists(cropInfo)) {
                Cultivo cultivo = new Cultivo() {
                    NombreCultivo = cropInfo.Name,
                    DescripcionCultivo = cropInfo.Description,
                    IdUsuario = cropInfo.UserID,
                    Estado = "A"
                };
                _context.Cultivos.Add(cultivo);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { message = "Cultivo agregado con exito" });
            } else {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Ya existe el cultivo" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditCrop([FromBody] EditCropInfo cropInfo) {
            Cultivo crop = _context.Cultivos.Where(u => u.IdCultivo == cropInfo.Id).FirstOrDefault();
            if (crop == null) {
                return StatusCode(StatusCodes.Status404NotFound, "Cultivo no encontrado");
            } else {
                crop.NombreCultivo = cropInfo.Name is null ? crop.NombreCultivo : cropInfo.Name;
                crop.DescripcionCultivo = cropInfo.Description is null ? crop.DescripcionCultivo : cropInfo.Description;
                _context.Cultivos.Update(crop);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "Cultivo actualizado con exito");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> DeleteCrop([FromQuery] int cropID) {
            Cultivo crop = _context.Cultivos.Where(c => c.IdCultivo == cropID).FirstOrDefault();
            if (crop is null) {
                return BadRequest("Cultivo no encontrado");
            } else {
                crop.Estado = "D";
                _context.Cultivos.Update(crop);
                await _context.SaveChangesAsync();
                return Ok("Cultivo eliminado con exito");
            }
        }

        private bool CropExists(CropInfo info) {
            return (_context.Cultivos?.Any(e => e.NombreCultivo == info.Name)).GetValueOrDefault();
        }

        private bool UserExists(int userID) {
            return (_context.Usuarios?.Any(e => e.IdUsuario == userID)).GetValueOrDefault();
        }

        public List<CropReport> GenerateCropsReports(List<Cultivo> cultivos) {
            List<CropReport> reports = new List<CropReport>();
            foreach (Cultivo cultivo in cultivos) {
                List<Gasto> cropExpenses = _context.Gastos.Where(g => g.IdCultivo == cultivo.IdCultivo && g.Estado == "A").ToList();
                List<Ingreso> cropIncomes = _context.Ingresos.Where(g => g.IdCultivo == cultivo.IdCultivo && g.Estado == "A").ToList();
                CropReport report = new CropReport() {
                    cropName = cultivo.NombreCultivo,
                    totalExpenses = cropExpenses.Sum(e => e.ValorGasto),
                    totalIncomes = cropIncomes.Sum(i => i.ValorIngreso),
                    totalLaborExpenses = cropExpenses.Where(e => e.TipoGasto == "MO").Sum(e => e.ValorGasto),
                    totalSuppliesExpenses = cropExpenses.Where(e => e.TipoGasto == "IN").Sum(e => e.ValorGasto),
                    totalOthersExpenses = cropExpenses.Where(e => e.TipoGasto == "OT").Sum(e => e.ValorGasto),
                };
                report.profit = report.totalIncomes - report.totalExpenses;
                reports.Add(report);
            }
            return reports;
        }
    }
}
