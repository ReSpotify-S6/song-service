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

var envStore = new EnvStore(requiredVariables);
builder.Services.AddSingleton<IReadOnlyDictionary<string, string>>(envStore);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ISongService, SongService.Services.SongService>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddSingleton<IKeycloakJwtHandler, KeycloakJwtHandler>();

builder.Services.AddDbContext<SongContext>();

// CORS
string corsPolicy = "frontend";

string[] allowedOrigins = envStore["ALLOWED_ORIGINS"].Split(',');

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
