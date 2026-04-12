using FluentValidation;
using LTS.API.Features.Benchs.Commands.CreateBench;

namespace LTS.API.Features.Benchs.Commands.CreateBench;

public class CreateBenchValidator : AbstractValidator<CreateBenchCommand>
{
    public CreateBenchValidator()
    {
        RuleFor(x => x.CaseId)
            .NotEmpty().WithMessage("CaseId required hai.");

        RuleFor(x => x.JudgeName)
            .NotEmpty().WithMessage("Judge ka naam required hai.")
            .MaximumLength(200).WithMessage("Judge naam 200 characters se zyada nahi ho sakta.");

        RuleFor(x => x.JudgeEmail)
            .EmailAddress().WithMessage("Valid email address dein.")
            .When(x => !string.IsNullOrEmpty(x.JudgeEmail));
    }
}