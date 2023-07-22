using BDTorneus;
using DTOs_Compartidos.DTOs;
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
using Utilidades;

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
                if (usuarioBuscado == null) throw new Exception("No existe el usuario o la contraseña es incorrecta");

                return usuarioBuscado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> LoginGoogleUsuario(LoginGoogleDTO loginGogole, string claveSecretaValidar)
        {
            try
            {
                if (loginGogole == null || string.IsNullOrEmpty(loginGogole.Mail) || string.IsNullOrEmpty(loginGogole.ClaveSecreta))
                {
                    throw new Exception("Hay campos sin completar");
                }

                if (claveSecretaValidar != loginGogole.ClaveSecreta) throw new Exception("No se ha podido validar tu cuenta Google");

                var usuarioBuscado = await _db.Usuarios.SingleOrDefaultAsync(us => us.Mail == loginGogole.Mail && us.Token == loginGogole.IdUsuarioGoogle);

                if (usuarioBuscado == null) throw new Exception(Util.REGISTRARSE_GOOGLE);

                return usuarioBuscado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> RegistroUsuario(Usuario registro)
        {
            try
            {
                string mensajeError = "";


                ValidadorUsuarioDuplicado validacionDuplicado = new(_db);
                ValidationResult resultDuplicado = validacionDuplicado.Validate(registro);
                if (!resultDuplicado.IsValid)
                {
                    resultDuplicado.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                ValidadorUsuarioRegistro validacion = new();
                ValidationResult result = validacion.Validate(registro);
                if (!result.IsValid)
                {
                    result.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                registro.AccessTokenMercadopago = "";
                registro.AccessTokenRefreshMercadopago = "";
                registro.Token = "";

                var usuarioNuevo = await _db.Usuarios.AddAsync(registro);
                await _db.SaveChangesAsync();

                Usuario usuarioLogueado = new()
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


        public async Task<Usuario> LoginUsuarioEspectador()
        {
            try
            {
                Usuario usuarioLogueado = new()
                {
                    Id = 0,
                    Mail = Guid.NewGuid().ToString(),
                    Rol = "ESPECTADOR",
                    Token = ""
                };


                return usuarioLogueado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> HabilitadoVendedorMercadoPago(int usuarioId)
        {
            try
            {
                var usuario = await _db.Usuarios.SingleOrDefaultAsync(us => us.Id == usuarioId);

                if (usuario == null) throw new Exception("No se encuentra el usuario. W106");

                bool usaMercadoPagoVendedor = !string.IsNullOrEmpty(usuario.AccessTokenMercadopago);

                return usaMercadoPagoVendedor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
