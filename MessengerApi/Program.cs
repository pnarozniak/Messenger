using MessengerApi.Database;
using MessengerApi.Options;
using MessengerApi.Repositories.ChatRepository;
using MessengerApi.Repositories.UserRepository;
using MessengerApi.Services.IEmailSender;
using MessengerApi.Services.ITokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure options
builder.Services.Configure<MySqlOptions>(builder.Configuration.GetSection("MySql"));
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Tokens"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

// Configure and initialize db context
MySqlOptions mySqlOptions = builder.Configuration.GetSection("MySql").Get<MySqlOptions>();
builder.Services.AddDbContext<AppDbContext>(options => 
    options
        .UseMySql(mySqlOptions.ConnectionString, ServerVersion.Parse(mySqlOptions.ServerVersion))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Configure authentication 
var accessTokenOptions = builder.Configuration.GetSection("Tokens").Get<TokenOptions>().AccessToken;
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = accessTokenOptions.ValidationParameters;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = accessTokenOptions.HandleAuthenticationFailed,
        };
    });

// Configure services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();

// Configure repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
