using API.Utility;
using Config.Net;
using Database;

var path = Path.GetFullPath("./config.json");
Console.WriteLine($"Path: {path}");

var settings = new ConfigurationBuilder<ISettings>()
    .UseJsonFile(path)
    .Build();

var connectionString = settings.ConnectionString;
Console.WriteLine($"Connection String: {connectionString}");

/*
var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;

    services.AddDbContext<DataContext>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
*/