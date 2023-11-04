using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; set; }
    DbSet<Company> Companies { get; set; }
    //DbSet<SystemLog<T>> SystemLogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}