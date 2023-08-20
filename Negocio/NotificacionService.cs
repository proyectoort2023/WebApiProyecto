using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NotificacionService
    {
        private readonly TorneoContext _db;

        public NotificacionService(TorneoContext db)
        {
            _db = db;
        }

        public async Task<Notificacion> Registrar(Notificacion notificacion)
        {
            try
            {
                if (notificacion == null) throw new Exception("No hay notificacion para registrar");
               
               _db.Entry(notificacion.Torneo).State = EntityState.Unchanged;
               _db.Entry(notificacion.Equipo).State = EntityState.Unchanged;

                var registrado = await _db.Notificaciones.AddAsync(notificacion);
                await _db.SaveChangesAsync();

                return registrado.Entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
