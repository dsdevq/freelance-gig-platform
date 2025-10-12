using FluentValidation;
using JobService.Application.Models;

namespace JobService.Application.Validators;

public class UpdateJobModelValidator : AbstractValidator<UpdateJobModel>
{
    public UpdateJobModelValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200);

        RuleFor(x => x.Budget)
            .GreaterThan(0)
            .When(x => x.Budget.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);
    }
}
