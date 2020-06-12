using System;
using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    public class CompleteCheckItemCommandValidator : AbstractValidator<CompleteCheckItemCommand>
    {
        public CompleteCheckItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.TrackerTypeId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LoggedTime)
                .NotEmpty()
                .InclusiveBetween(0.1f, 24);

            RuleFor(x => x.DateOfWorkValue)
                .GreaterThan(DateTime.MinValue);
        }
    }
}
