using Database;

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
