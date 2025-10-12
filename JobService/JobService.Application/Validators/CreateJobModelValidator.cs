using FluentValidation;
using JobService.Application.Models;

namespace JobService.Application.Validators;

public class CreateJobModelValidator : AbstractValidator<CreateJobModel>
{
    public CreateJobModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be positive.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.");
    }
}