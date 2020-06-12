using System;
using FluentValidation;
using Gear.Domain.ReportEntities.Enums;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportDate
{
    class AddUserReportDateCommandValidator<TUserIdType> : AbstractValidator<AddUserReportDateCommand<TUserIdType>>
    {
        public AddUserReportDateCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.FilterType)
                .Must(x => FilterType.StartDate == x || FilterType.DueDate == x);

            RuleFor(x => x.Date)
                .GreaterThan(DateTime.MinValue)
                .LessThan(DateTime.MaxValue);
        }
    }
}
