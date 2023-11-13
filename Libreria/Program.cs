using Microsoft.EntityFrameworkCore;
using Libreria.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibreriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibreriaContext") ?? throw new InvalidOperationException("Connection string 'LibreriaContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregamos la politica de seguridad CORS para aceptar las
//peticiones que se realices desde cualqier servidor
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy => policy.WithOrigins
    ("*").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
