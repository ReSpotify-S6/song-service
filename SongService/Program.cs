using SongService;
using SongService.Authorization;
using SongService.Repository;
using SongService.Services;

var builder = WebApplication.CreateBuilder(args);

var requiredVariables = new List<string>
{
    "RABBITMQ_HOSTNAME",
    "RABBITMQ_USERNAME",
    "RABBITMQ_PASSWORD",

    "DB_HOST", 
    "DB_PORT", 
    "DB_USERNAME", 
    "DB_PASSWORD", 
    "DB_DATABASE",

    "ALLOWED_ORIGINS",
    "KC_JWKS_URL",
    "API_GATEWAY_HOST",
};


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
var envManager = new EnvironmentVariableManager(requiredVariables);
builder.Services.AddSingleton(envManager);

builder.Services.AddSingleton<ISongService, SongService.Services.SongService>();
builder.Services.AddSingleton<ISongRepository, SongRepository>();
builder.Services.AddSingleton<IKeycloakJwtHandler, KeycloakJwtHandler>();

builder.Services.AddDbContext<SongContext>();



string[] allowedOrigins = envManager["ALLOWED_ORIGINS"].Split(',');
string corsPolicy = "frontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicy,
        policy => {
            policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.UseMiddleware<AuthMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // For tests