using FluentValidation.TestHelper;
using LTS.API.Features.Courts.Commands.CreateCourt;
using Xunit;

namespace LTS.Tests.Features.Courts.Commands.Tests.CreateCourt;

public class CreateCourtValidatorTests
{
    private readonly CreateCourtValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_CourtName_Is_Empty()
    {
        var command = new CreateCourtCommand("", "Valid Address");

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CourtName);
    }

    [Fact]
    public void Should_Have_Error_When_CourtName_Exceeds_100_Characters()
    {
        var command = new CreateCourtCommand(new string('A', 101), "Valid Address");

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CourtName);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateCourtCommand("Session Court", "Valid Address");

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}