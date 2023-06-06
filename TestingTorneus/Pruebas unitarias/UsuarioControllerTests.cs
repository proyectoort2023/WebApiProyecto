using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Negocio;
using Negocio.DTOs;
using WebApiTorneus.Controllers;

namespace TestingTorneus
{
    [TestClass]
    public class UsuarioControllerTests : BasePruebas
    {
       
       

        [TestMethod]
        public async Task PostLogin_ReturnsOk()
        {
            string nombreBD = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBD);
            
            var mapper = ConfigurarAutoMapper();
            var config = MiConfiguracionTest();

            
            // Arrange
            var loginDTO = new LoginDTO
            {
                Mail = "alex@gmail.com",
                Pass = "string123"
            };

            // Insertar un usuario de prueba en la base de datos en memoria
            var usuario = new Usuario
            {
                Mail = "alex@gmail.com",
                Pass = "string123"
            };
            dbContext.Usuarios.Add(usuario);
            int op = await dbContext.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var usuarioService = new UsuarioService(contexto2);
            var usuarioController = new UsuarioController(mapper, usuarioService, config);

            // Act
            var result = await usuarioController.PostLogin(loginDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task PostLogin_ReturnsBadRequest()
        {
            string nombreBD = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBD);
            var usuarioService = new UsuarioService(dbContext);
            var mapper = ConfigurarAutoMapper();
            var config = MiConfiguracionTest();

            var usuarioController = new UsuarioController(mapper, usuarioService, config);
            // Arrange
            var loginDTO = new LoginDTO
            {
                Mail = "alexf@gmail.com",
                Pass = "dsfsdf54"
            };

            // Act
            var result = await usuarioController.PostLogin(loginDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}