﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Models
{
    public class ImagenModel
    {
        public IFormFile Archivo { get; set; }
        public string NombreArchivoAnterior { get; set; }
    }
}
