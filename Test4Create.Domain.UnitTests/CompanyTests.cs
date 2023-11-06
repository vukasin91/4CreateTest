using Domain.Entities;
using FluentAssertions;

namespace Test4Create.Domain.UnitTests;

public class CompanyTests
{
    [Fact]
    public void Create_ShouldCreateCompany_WhenGivenValidName()
    {
        // Arrange
        var companyName = "Acme Corporation";

        // Act
        var company = Company.Create(companyName);

        // Assert
        company.Should().NotBeNull();
        company.Name.Should().Be(companyName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_ShouldThrowArgumentException_WhenGivenInvalidName(string invalidName)
    {
        if (invalidName is null)
        {
            Assert.Throws<ArgumentNullException>(() => Company.Create(invalidName));

        }
        else
        {
            Assert.Throws<System.ArgumentException>(() => Company.Create(invalidName));
        }
    }
}