using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitializer> _logger;

    public ApplicationDbContextInitializer(ApplicationDbContext context, ILogger<ApplicationDbContextInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while applying migrations", ex.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while seeding db", ex.Message);
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Companies.Any())
        {
            _context.Companies.Add(Company.Create("Company 1"));
            _context.Companies.Add(Company.Create("Company 2"));
            _context.Companies.Add(Company.Create("Company 3"));
        }

        if (!_context.Employees.Any())
        {
            _context.Employees.Add(Employee.Create("John", "Doe", "john.doe@mail.com", Domain.Enums.EmployeeType.Developer, new List<Company> { Company.Create("Company 1") }));
        }
        await _context.SaveChangesAsync();
    }
}