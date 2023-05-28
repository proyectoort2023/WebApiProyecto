using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ImagenService
    {
        private string _extension = ".png";
        public async Task<string> SubirImagen(string keyAzureBlob, string storageNameAzureBlob, IFormFile imagenStream)
        {
            try
            {
                await using var memoryStreamImagen = new MemoryStream();
                await imagenStream.CopyToAsync(memoryStreamImagen);
                memoryStreamImagen.Position = 0;

                string imageName = Guid.NewGuid().ToString() + _extension;

                BlobContainerClient blobContainer = new BlobContainerClient(keyAzureBlob, storageNameAzureBlob);
                BlobClient blob = blobContainer.GetBlobClient(imageName);

                await blob.UploadAsync(memoryStreamImagen);

                return imageName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ActualizarImagen(string keyAzureBlob, string storageNameAzureBlob, IFormFile imagenStream, string nombreArchivoAnterior)
        {
            try
            {
                await using var memoryStreamImagen = new MemoryStream();
                await imagenStream.CopyToAsync(memoryStreamImagen);
                memoryStreamImagen.Position = 0;

                string imageName = Guid.NewGuid().ToString() + _extension;

                BlobContainerClient blobContainer = new BlobContainerClient(keyAzureBlob, storageNameAzureBlob);


                BlobClient blobAEliminar = blobContainer.GetBlobClient(nombreArchivoAnterior);
                await EliminarImagen(blobAEliminar);


                BlobClient blob = blobContainer.GetBlobClient(imageName);
                await blob.UploadAsync(memoryStreamImagen);

                return imageName;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task EliminarImagen(BlobClient blob)
        {
            try
            {
                await blob.DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }
}
