using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class ImagenServicio
    {
        private readonly HttpClient _httpClient;
        private long maxSizeFile = long.MaxValue;
        private string _format = "image/png";
        private readonly ImagenServicioDatos _imagenServicioDatos;

        int ancho, alto;

        public ImagenServicio(HttpClient _httpClient, ImagenServicioDatos imagenServicioDatos)
        {
            this._httpClient = _httpClient;
            _imagenServicioDatos = imagenServicioDatos;
            ancho = 600;
            alto = 600;
        }


        public async Task<string> SubirImagenFile(InputFileChangeEventArgs e)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                foreach (var image in e.GetMultipleFiles(1))
                {
                    var imagenRedimensionado = await image.RequestImageFileAsync(_format, ancho, alto);

                    var memoryStream = new MemoryStream();
                    await imagenRedimensionado.OpenReadStream(maxSizeFile).CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var fileContent = new StreamContent(memoryStream,(int)memoryStream.Length);

                    string fileName = image.Name;
                    formData.Add(fileContent, "imagenModel", fileName);
                }
                string resultado = await _imagenServicioDatos.SubirImagenFile(formData);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ActualizarImagenFile(InputFileChangeEventArgs e, string nombreArchivoActual)
        {
            try
            {
                var formData = new MultipartFormDataContent();

                foreach (var image in e.GetMultipleFiles(1))
                {
                    var imagenRedimensionado = await image.RequestImageFileAsync(_format, ancho, alto);

                    var memoryStream = new MemoryStream();
                    await imagenRedimensionado.OpenReadStream(maxSizeFile).CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var fileContent = new StreamContent(memoryStream, (int)memoryStream.Length);

                    formData.Add(fileContent, "imagenModel", nombreArchivoActual);
                }

                string resultado = await _imagenServicioDatos.ActualizarImagenFile(formData,nombreArchivoActual);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
