using BDTorneus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Negocio;
using System.Reflection;
using System.Text;
using WebApiTorneus.AMProfile;
using WebApiTorneus.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews().AddNewtonsoftJson();

builder.Services.AddControllers();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nombre de tu API", Version = "v1" });

    // Ruta del archivo XML de comentarios
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Incluir comentarios XML
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddCors(opciones => {
    opciones.AddPolicy("cors", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyOrigin();
    }
    );
});
string cadenaConexionBDLocal = "LocalConnection";
string cadenaConexionAzureBD = "ProduccionConnection";

builder.Services.AddDbContext<TorneoContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString(cadenaConexionBDLocal));
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHostedService<TareaSegundoPlanoTorneo>();

//builder.Services.AddSignalR();
//builder.Services.AddResponseCompression(opts =>
//{
//    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
//        new[] { "application/octet-stream" });
//});

//Dependencias de Negocio
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TorneoService>();
builder.Services.AddScoped<ImagenService>();


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
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<TorneoContext>();
//    db.Database.Migrate();
//}

app.UseCors("cors");
//app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapHub<PartidoHub>("/partidohubs");

app.Run();
