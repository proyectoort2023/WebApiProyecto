using BDTorneus;
using FluentValidation.Results;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioService
    {
        private readonly TorneoContext _db;
        
        public UsuarioService(TorneoContext db)
        {
            _db = db;
        }

        public async Task<Usuario> LoginUsuario(LoginDTO login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.Mail) || string.IsNullOrEmpty(login.Pass))
                {
                    throw new Exception("Hay campos sin completar");
                }

                var usuarioBuscado = await _db.Usuarios.SingleOrDefaultAsync(us => us.Mail == login.Mail && 
                                                                                    us.Pass == login.Pass);
                if (usuarioBuscado == null)  throw new Exception("No existe el usuario o la contraseña es incorrecta");

                return usuarioBuscado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioLogueado> RegistroUsuario(Usuario registro)
        {
            try
            {
                string mensajeError = "";

                ValidadorUsuarioRegistro validacion = new(_db);
                ValidationResult result = validacion.Validate(registro);
                if (!result.IsValid)
                {
                    result.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                var usuarioNuevo = await _db.AddAsync(registro);
                await _db.SaveChangesAsync();

                UsuarioLogueado usuarioLogueado = new()
                {
                    Id = usuarioNuevo.Entity.Id,
                    Mail = usuarioNuevo.Entity.Mail,
                    Rol = usuarioNuevo.Entity.Rol,
                    Token = usuarioNuevo.Entity.Token
                };
                return usuarioLogueado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
