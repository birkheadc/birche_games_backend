using BircheGamesApi.Config;
using BircheGamesApi.Repositories;
using BircheGamesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Todo: Do this in another class or something, maybe DatabaseConfig's constructor
DatabaseConfig databaseConfig;
if (builder.Environment.IsDevelopment())
{
  databaseConfig = builder.Configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
  Console.WriteLine("Connection String: " + databaseConfig.ConnectionString);
}
else
{
  databaseConfig = new DatabaseConfig()
  {
    ConnectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING"),
    DatabaseName = Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE_NAME"),
    GamesCollectionName = Environment.GetEnvironmentVariable("ASPNETCORE_GAMES_COLLECTION_NAME")
  };
}

// Add services to the container.

builder.Services.AddSingleton(databaseConfig);

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameProfileRepository, GameProfileRepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
  options =>
  {
    options.AddPolicy(
      name: "All",
      builder =>
      {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
      }
    );
  });

var app = builder.Build();

app.UseCors("All");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapGet("/api/rootpath", (HttpContext context) =>
{
    string host = context.Request.Host.ToUriComponent();
    var url =  $"{context.Request.Scheme}://{host}";
    return url;
});

app.UseAuthorization();

app.MapControllers();

app.Run();
