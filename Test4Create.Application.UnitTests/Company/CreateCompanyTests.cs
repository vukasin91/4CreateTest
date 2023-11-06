using Application.Common.Interfaces;
using Application.Companies.Commands.CreateCompany;
using Application.Employees.Commands.CreateEmployee;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test4Create.Application.UnitTests.Company;

public class CreateCompanyTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CreateCompanyCommandHandler>> _mockLogger;
    private readonly Mock<DbSet<Domain.Entities.Company>> _mockCompanyDbSet;
    private readonly Mock<DbSet<Domain.Entities.Employee>> _mockEmployeeDbSet;

    public CreateCompanyTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CreateCompanyCommandHandler>>();
        _mockCompanyDbSet = new Mock<DbSet<Domain.Entities.Company>>();
        _mockEmployeeDbSet = new Mock<DbSet<Domain.Entities.Employee>>();

        _mockContext.SetupGet(x => x.Companies).Returns(_mockCompanyDbSet.Object);
        _mockContext.SetupGet(x => x.Employees).Returns(_mockEmployeeDbSet.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldCreateCompany()
    {
        // Arrange
        var handler = new CreateCompanyCommandHandler(_mockContext.Object, _mockMediator.Object, _mockLogger.Object);
        var companyName = "New Company";
        var employeesDto = new List<EmployeeDto>
        {
            new EmployeeDto("John", "Doe", "john@example.com", "Developer", 1)
        };
        var command = new CreateCompanyCommand(companyName, employeesDto);

        _mockCompanyDbSet.Setup(x => x.Add(It.IsAny<Domain.Entities.Company>()));

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockCompanyDbSet.Verify(x => x.Add(It.IsAny<Domain.Entities.Company>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}