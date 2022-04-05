using MessengerApi.Database;
using MessengerApi.Services.TokenService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure and initialize db context
builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseMySql(
        builder.Configuration["MySql:ConnectionString"], 
        new MySqlServerVersion(new Version(builder.Configuration["MySql:ServerVersion"]))
    )
);

//Configure services
builder.Services.AddScoped<ITokenService, TokenService>();

//Configure repositories


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
