var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
