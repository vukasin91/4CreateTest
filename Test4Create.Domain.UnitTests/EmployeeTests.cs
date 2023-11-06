using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Test4Create.Domain.UnitTests;

public class EmployeeTests
{
    [Fact]
    public void CreateEmployee_WithValidEmailAndTitle_ShouldCreateEmployee()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "test@example.com";
        var title = EmployeeType.Developer;

        // Act
        var employee = Employee.Create(firstName, lastName, email, title, null);

        // Assert
        employee.Email.Should().Be(email);
        employee.Title.Should().Be(title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateEmployee_WithInvalidEmail_ShouldThrowArgumentException(string email)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var title = EmployeeType.Developer;

        // Act & Assert
        if (email is null)
        {
            Assert.Throws<System.ArgumentNullException>(() => Employee.Create(firstName, lastName, email, title, null));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => Employee.Create(firstName, lastName, email, title, null));
        }
    }
}