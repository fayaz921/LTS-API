using LTS.API.Features.Courts.Commands.CreateCourt;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LTS.Tests.Features.Courts.Commands.Tests.CreateCourt;

public class CreateCourtHandlerTests
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        // Har test k liye unique in-memory database banay ga
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_WhenCourtIsSaved_ShouldReturnSuccessResponse()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var handler = new CreateCourtHandler(context);
        var command = new CreateCourtCommand("High Court", "Lahore");

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotEqual(Guid.Empty, response.Data);

        // Database check
        var courtInDb = await context.Courts.FindAsync(response.Data);
        Assert.NotNull(courtInDb);
    }

    [Fact]
    public async Task Handle_WhenCourtIsSaved_ShouldExistInDatabase()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var handler = new CreateCourtHandler(context);
        var command = new CreateCourtCommand("Session Court", "Peshawar");

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.IsSuccess);
        var court = await context.Courts.FirstOrDefaultAsync(x => x.CourtName == "Session Court");
        Assert.NotNull(court);
    }
}