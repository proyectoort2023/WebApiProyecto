using BDTorneus;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TorneoService
    {
        private readonly TorneoContext _db;
        private readonly IConfiguration _configuration;

        public TorneoService(TorneoContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }


        public async Task<int> CrearTorneo(Torneo torneo)
        {
            try
            {
                string mensajeError = "";

                ValidadorTorneo validacion = new(_db, _configuration);
                ValidationResult result = validacion.Validate(torneo);
                if (!result.IsValid)
                {
                    result.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                var torneoNuevo = await _db.Torneos.AddAsync(torneo);
                await _db.SaveChangesAsync();

                return torneoNuevo.Entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
