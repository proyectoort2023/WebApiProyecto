using BDTorneus;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
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
                if (torneo == null)  throw new Exception("El torneo no tiene datos para salvar"); 
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


        public async Task<bool> SuspenderTorneo(int torneoId)
        {
            try
            {
                Torneo torneo = await _db.Torneos.FindAsync(torneoId);

                if (torneo == null) throw new Exception("El torneo no tiene datos para salvar");
               
                torneo.HabilitacionInscripcion = false;
                torneo.Suspendido = true;

                int updateRealizado = await _db.SaveChangesAsync();

                // Hay que resolver el tema del reembolso de aquellos que pagaron por pasarela de pago online
                return updateRealizado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> EliminarTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) throw new Exception("El torneo que quiere eliminar no existe");
                
                Torneo torneoAEliminar = await _db.Torneos.Include(i => i.Inscripciones)
                                                           .SingleOrDefaultAsync(t => t.Id == torneoId);

                if (torneoAEliminar == null) throw new Exception("El torneo que quiere eliminar no existe");
                 if (torneoAEliminar.Inscripciones.Count > 0) throw new Exception("No puede eliminar porque hay equipos inscriptos");
                _db.Torneos.Remove(torneoAEliminar);
                int eliminacionRealizada = await _db.SaveChangesAsync();

                // Hay que resolver el tema del reembolso de aquellos que pagaron por pasarela de pago online
                return eliminacionRealizada > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
