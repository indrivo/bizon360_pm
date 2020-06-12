using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.CreateDepartmentTeam
{
    public class CreateDepartmentTeamCommandValidator : AbstractValidator<CreateDepartmentTeamCommand>
    {
        public CreateDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.Description).MaximumLength(900);

            RuleFor(x => x.Name).MaximumLength(256)
            .NotEmpty();
        }
    }
}
