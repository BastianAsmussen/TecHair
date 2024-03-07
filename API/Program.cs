using API.Utility;
using API.Utility.Database;
using Config.Net;

namespace API;

public class Program
{
    public static readonly ISettings Settings = new ConfigurationBuilder<ISettings>()
        .UseJsonFile(Path.GetFullPath("config.json"))
        .Build();

    public static void Main(string[] args)
    {
        if (Settings.ConnectionString == null) throw new InvalidOperationException("Connection string not found!");

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
    }
}