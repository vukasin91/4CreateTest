using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            //_logger
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
            //log
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Employees.Any())
        {
            //_context.Employees.Add();
        }

        if (!_context.Companies.Any())
        {
            //_context.Companies.Add();
        }

        await _context.SaveChangesAsync();
    }
}