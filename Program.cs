using System.Text;
using BircheGamesApi.Config;
using BircheGamesApi.Models;
using BircheGamesApi.Repositories;
using BircheGamesApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Todo: Do this in another class or something, maybe DatabaseConfig's constructor
DatabaseConfig databaseConfig;
if (builder.Environment.IsDevelopment())
{
  databaseConfig = builder.Configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
}
else
{
  databaseConfig = new DatabaseConfig()
  {
    ConnectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING"),
    DatabaseName = Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE_NAME"),
    GamesCollectionName = Environment.GetEnvironmentVariable("ASPNETCORE_GAMES_COLLECTION_NAME"),
    PasswordCollectionName = Environment.GetEnvironmentVariable("ASPNETCORE_PASSWORD_COLLECTION_NAME")
  };
}

JwtConfig jwtConfig;
if (builder.Environment.IsDevelopment())
{
  jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>();
}
else
{
  jwtConfig = new JwtConfig()
  {
    Issuer = Environment.GetEnvironmentVariable("ASPNETCORE_JWT_ISSUER"),
    Audience = Environment.GetEnvironmentVariable("ASPNETCORE_JWT_AUDIENCE"),
    Key = Environment.GetEnvironmentVariable("ASPNETCORE_JWT_KEY")
  };
}

// Set default Admin password in database if password doesn't exist
try
{
  MongoClient mongoClient = new(databaseConfig.ConnectionString);
  IMongoDatabase db = mongoClient.GetDatabase(databaseConfig.DatabaseName);
  IMongoCollection<PasswordModel> passwordCollection = db.GetCollection<PasswordModel>(databaseConfig.PasswordCollectionName);

  if (passwordCollection.CountDocuments(_ => true) == 0)
  {
    string password;
    if (builder.Environment.IsDevelopment())
    {
      password = builder.Configuration.GetSection("AdminPassword").Get<PasswordModel>().Password;
    }
    else
    {
      password = Environment.GetEnvironmentVariable("ASPNETCORE_ADMIN_PASSWORD") ?? "";
    }
    string hash = BCrypt.Net.BCrypt.HashPassword(password);
    PasswordModel passwordModel = new() { Password = hash };
    passwordCollection.InsertOne(passwordModel);
  }
}
catch
{
  throw new Exception("Unable to connect to database. Aborting program.");
}


// Add services to the container.

builder.Services.AddSingleton(databaseConfig);
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors
(
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

builder.Services.AddAuthentication
(
  options =>
  {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

  }
).AddJwtBearer
(
  o =>
  {
    o.TokenValidationParameters = new()
    {
      ValidIssuer = jwtConfig.Issuer,
      ValidAudience = jwtConfig.Audience,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key ?? "")),
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true
    };
  }
);

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();