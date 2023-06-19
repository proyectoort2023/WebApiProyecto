﻿using Blazored.LocalStorage;
using Negocio.DTOs;
using System.IdentityModel.Tokens.Jwt;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class UsuarioServicio
    {
        private UsuarioLogueado _usuarioLogueado;
        private readonly UsuarioServicioDatos _usuarioServicioDatos;

        public UsuarioServicio(UsuarioServicioDatos usuarioServicioDatos)
        {
            _usuarioServicioDatos = usuarioServicioDatos;
        }
        public void ActualizarUsuarioLogueado(UsuarioLogueado usuario)
        {
            _usuarioLogueado = usuario;
        }

        public UsuarioLogueado ObtenerUsuarioLogueado()
        {
            return _usuarioLogueado;
        }

        public async Task<bool> Loguearme(LoginDTO loginDTO)
        {
            try
            {
               string tokenRecibido = await  _usuarioServicioDatos.LoginUsuario(loginDTO);
                if (string.IsNullOrEmpty(tokenRecibido)) return false;

                UsuarioLogueado usuario = await DecodificarUsuarioJWT(tokenRecibido);
                _usuarioLogueado = usuario;

                
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public async Task<UsuarioLogueado> DecodificarUsuarioJWT(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenString = token.Replace("Bearer ", ""); // Si el token incluye el prefijo "Bearer", puedes removerlo aquí

            if (handler.CanReadToken(tokenString))
            {
                var jwtToken = handler.ReadJwtToken(tokenString);
                var claims = jwtToken.Claims;

                // Acceder a los claims del token
                var id = claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value;
                int idUsuario = Int32.Parse(id);
                var email = claims.FirstOrDefault(c => c.Type == "Email")?.Value;
                var rol = claims.FirstOrDefault(c => c.Type == "Rol")?.Value;

                UsuarioLogueado usuario = new()
                {
                    Id = idUsuario,
                    Mail = email,
                    Rol = rol,
                    Token = token
                };
                return usuario;
            }
            else
            {
                return null;
            }
        }










    }
}
