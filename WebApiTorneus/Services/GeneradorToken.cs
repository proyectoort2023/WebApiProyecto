using BDTorneus;
using Microsoft.IdentityModel.Tokens;
using Negocio.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiTorneus.Services
{
    public static class GeneradorToken
    {
        public static string CrearToken(UsuarioLogueado usuarioLogueado, IConfiguration _config)
        {
            var claveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credenciales = new SigningCredentials(claveSeguridad, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("IdUsuario" , usuarioLogueado.Id.ToString()),
                new Claim("Email" , usuarioLogueado.Mail),
                new Claim("Rol", usuarioLogueado.Rol),
                new Claim(ClaimTypes.Role,usuarioLogueado.Rol)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddYears(3),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
