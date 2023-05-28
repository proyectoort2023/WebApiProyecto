﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class Inscripcion
    {
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }
        public double Monto { get; set; }
        public string MedioPago { get; set; }
        public string Estado { get; set; }
    }
}
