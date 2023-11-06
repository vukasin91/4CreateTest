using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Test4Create.Domain.UnitTests;

public class SystemLogsTests
{
    [Fact]
    public void Constructor_ShouldCreateSystemLog_WhenGivenValidEvent()
    {
        // Arrange
        var entityType = "Employee";
        var id = 1;
        EventType eventType = EventType.Create;
        var attributes = "Some attributes";
        var comment = "Some comment";

        // Act
        var log = SystemLog.Create(
            entityType,
            id,
            eventType,
            attributes,
            comment);

        // Assert
        log.Should().NotBeNull();
        log.Event.Should().Be(eventType);
        log.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenGivenEmptyEntityType()
    {
        // Arrange
        var id = 1;
        EventType eventType = EventType.Get;
        var attributes = "Some attributes";
        var comment = "Some comment";

        // Assert
       Assert.Throws<System.ArgumentException>(() => SystemLog.Create(
            "",
            id,
            eventType,
            attributes,
            comment));
    }
}