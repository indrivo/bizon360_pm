using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AssignOwnerToCompany
{
    public class AssignCompanyOwnerCommandValidator : AbstractValidator<AssignCompanyOwnerCommand>
    {
        public AssignCompanyOwnerCommandValidator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.CompanyId)
                .NotEmpty()
                .NotNull();
        }
    }
}
