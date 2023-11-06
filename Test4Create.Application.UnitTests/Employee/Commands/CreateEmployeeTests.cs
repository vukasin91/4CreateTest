using Application.Common.Interfaces;
using Application.Employees.Commands.CreateEmployee;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test4Create.Application.UnitTests.Employee.Commands;

public class CreateEmployeeTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _mockLogger;
    private readonly Mock<DbSet<Domain.Entities.Company>> _mockCompanyDbSet;
    private readonly Mock<DbSet<Domain.Entities.Employee>> _mockEmployeeDbSet;

    public CreateEmployeeTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CreateEmployeeCommandHandler>>();
        _mockCompanyDbSet = new Mock<DbSet<Domain.Entities.Company>>();
        _mockEmployeeDbSet = new Mock<DbSet<Domain.Entities.Employee>>();

        _mockContext.SetupGet(x => x.Companies).Returns(_mockCompanyDbSet.Object);
        _mockContext.SetupGet(x => x.Employees).Returns(_mockEmployeeDbSet.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCreateEmployee()
    {
        // Arrange
        var companiesIds = new List<int> { 1 };
        var companies = new List<Domain.Entities.Company>
        {
           Domain.Entities.Company.Create("Company A")
        }.AsQueryable();

        _mockCompanyDbSet.As<IQueryable<Domain.Entities.Company>>().Setup(m => m.Provider).Returns(companies.Provider);
        _mockCompanyDbSet.As<IQueryable<Domain.Entities.Company>>().Setup(m => m.Expression).Returns(companies.Expression);
        _mockCompanyDbSet.As<IQueryable<Domain.Entities.Company>>().Setup(m => m.ElementType).Returns(companies.ElementType);
        _mockCompanyDbSet.As<IQueryable<Domain.Entities.Company>>().Setup(m => m.GetEnumerator()).Returns(companies.GetEnumerator());

        _mockContext.Setup(c => c.Companies).Returns(_mockCompanyDbSet.Object);

        var handler = new CreateEmployeeCommandHandler(_mockContext.Object, _mockMediator.Object, _mockLogger.Object);
        var command = new CreateEmployeeCommand(
            companies.Select(c => c.Id), "John", "Doe", "john.doe@example.com", EmployeeType.Developer);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}