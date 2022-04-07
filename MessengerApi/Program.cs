using MessengerApi.Database;
using MessengerApi.Options;
using MessengerApi.Services.IEmailSender;
using MessengerApi.Services.ITokenService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure options
builder.Services.Configure<MySqlOptions>(builder.Configuration.GetSection("MySql"));
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Tokens"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

//Configure and initialize db context
builder.Services.AddDbContext<AppDbContext>();

//Configure services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();

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
