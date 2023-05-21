using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Negocio;
using WebApiTorneus.AMProfile;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opciones => {
    opciones.AddPolicy("cors", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyOrigin();
    }
    );
});
string cadenaConexionBD = "LocalConnection";
string cadenaConexionAzureBD = "ProduccionConnection";

builder.Services.AddDbContext<TorneoContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString(cadenaConexionAzureBD));
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//builder.Services.AddSignalR();
//builder.Services.AddResponseCompression(opts =>
//{
//    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
//        new[] { "application/octet-stream" });
//});

//Dependencias de Negocio
builder.Services.AddScoped<UsuarioService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();



//creacion o acceso a la base de datos a traves de las migraciones
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<TorneoContext>();
dbContext.Database.Migrate();

app.UseCors("cors");
//app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapHub<PartidoHub>("/partidohubs");

app.Run();
