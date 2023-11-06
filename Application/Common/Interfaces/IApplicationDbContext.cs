using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}