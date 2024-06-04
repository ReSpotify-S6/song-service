using SongService;
using SongService.Repository;
using SongService.Services;

var builder = WebApplication.CreateBuilder(args);

string[] allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',') ?? ["localhost"];
string corsPolicy = "frontend";

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISongService, SongService.Services.SongService>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddSingleton<IKeycloakJwtHandler, KeycloakJwtHandler>();

builder.Services.AddDbContext<SongContext>();

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