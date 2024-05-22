using SongService.Repository;
using SongService.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "frontend",
        policy => {
            policy.WithOrigins("http://respotify.com")
                  .WithOrigins("http://localhost")
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
builder.Services.AddDbContext<SongContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // For tests