using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Solanum_back.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Solanum_back.Controllers {

    [Route("solanum/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {

        private readonly SolanumContext _context;
        private readonly string secretKey;

        public LoginController(IConfiguration config, SolanumContext context) {
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            this._context = context;    
        }

        [Route("Access")]
        [HttpPost]
        public async Task<IActionResult> Login([FromQuery] string userName, [FromQuery] string password) {
            Usuario user = await GetUser(userName, password);
            if (user != null) {
                DateTime expirationDate = DateTime.UtcNow.AddMinutes(60);
                string newToken = GenerateToken(userName, expirationDate);
                return StatusCode(StatusCodes.Status200OK, new { access = true, token = newToken, userID = user.IdUsuario,
                                                                names = user.Nombres + " " + user.Apellidos });
            } else {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Credenciales incorrectas" });
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterInfo info) {
            if (!UserExists(info)) {
                Usuario user = new Usuario() {
                    Nombres = info.Names,
                    Apellidos = info.LastNames,
                    NombreUsuario = info.UserName,
                    Contrasenia = info.Password,
                    Estado = "A"
                };
                Usuario createdUser = _context.Usuarios.Add(user).Entity;
                await _context.SaveChangesAsync();
                DateTime expirationDate = DateTime.UtcNow.AddMinutes(60);
                string newToken = GenerateToken(user.NombreUsuario, expirationDate);
                return StatusCode(StatusCodes.Status200OK, new { 
                                                                token = newToken ,
                                                                message = "Usuario registrado con exito", 
                                                                userID = createdUser.IdUsuario,
                                                                names = createdUser.Nombres + " " + createdUser.Apellidos
                                                                });;
            } else {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Ya existe el usuario" });
            }
        }

        private bool UserExists(RegisterInfo info) {
            return (_context.Usuarios?.Any(e => e.NombreUsuario == info.UserName)).GetValueOrDefault();
        }

        private async Task<Usuario> GetUser(string userName, string password) {
            return _context.Usuarios.Where(e => e.NombreUsuario == userName && e.Contrasenia == password).FirstOrDefault();
        }

        private string GenerateToken(string userName, DateTime expirationDate) {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userName));

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = claims,
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(tokenConfig);
            return token;
        }
    }
}
