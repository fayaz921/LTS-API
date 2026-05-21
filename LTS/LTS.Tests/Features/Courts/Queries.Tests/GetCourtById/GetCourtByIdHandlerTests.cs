using LTS.API.Domain.Entities;
using LTS.API.Features.Courts.Queries.GetCourtById;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LTS.Tests.Features.Courts.Queries.Tests.GetCourtById;

public class GetCourtByIdHandlerTests
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_WhenCourtExistsAndIsActive_ShouldReturnCourtDto()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var targetId = Guid.NewGuid();

        context.Courts.AddRange(new List<Court>
        {
            new() { Id = targetId, CourtName = "Supreme Court", IsActive = true },
            new() { Id = Guid.NewGuid(), CourtName = "District Court", IsActive = true }
        });
        await context.SaveChangesAsync();

        var handler = new GetCourtByIdHandler(context);
        var query = new GetCourtByIdQuery(targetId);

        // Act
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal("Supreme Court", response.Data.CourtName);
    }

    [Fact]
    public async Task Handle_WhenCourtIsInactive_ShouldReturnNotFound()
    {
        // Arrange
        using var context = new AppDbContext(CreateNewContextOptions());
        var targetId = Guid.NewGuid();

        context.Courts.Add(new Court { Id = targetId, CourtName = "Closed Court", IsActive = false });
        await context.SaveChangesAsync();

        var handler = new GetCourtByIdHandler(context);
        var query = new GetCourtByIdQuery(targetId);

        // Act
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Equal("Court not found", response.Message);
    }
}