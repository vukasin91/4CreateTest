using Application;
using Infrastructure;
using Infrastructure.Data;
using Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddPresentation();

builder.Host
    .UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<ApplicationDbContextInitializer>>();
    var initializer = new ApplicationDbContextInitializer(
        services.GetRequiredService<ApplicationDbContext>(), logger);

    // Initialize and seed the database
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCompanyEndpoints();
app.AddEmployeeEndpoints();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.Run();