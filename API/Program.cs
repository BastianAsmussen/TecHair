using API.Utility;
using Config.Net;
using Database;

var path = Path.GetFullPath("./config.json");
var settings = new ConfigurationBuilder<ISettings>()
    .UseJsonFile(path)
    .Build();

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;

    services.AddDbContext<DataContext>();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();

app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
