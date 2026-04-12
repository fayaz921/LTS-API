using FluentValidation;

namespace LTS.API.Features.Benchs.Commands.UpdateBench;

public class UpdateBenchValidator : AbstractValidator<UpdateBenchCommand>
{
    public UpdateBenchValidator()
    {
        RuleFor(x => x.BenchId)
            .NotEmpty().WithMessage("BenchId required hai.");

        RuleFor(x => x.JudgeName)
            .NotEmpty().WithMessage("Judge ka naam required hai.")
            .MaximumLength(100).WithMessage("Judge naam 100 se zyada characters nahi ho sakte.");

        RuleFor(x => x.JudgeEmail)
            .EmailAddress().WithMessage("Valid email address dein.")
            .When(x => !string.IsNullOrEmpty(x.JudgeEmail));
    }
}