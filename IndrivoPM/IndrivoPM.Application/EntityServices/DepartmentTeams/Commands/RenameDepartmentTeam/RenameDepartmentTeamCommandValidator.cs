using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RenameDepartmentTeam
{
    public class RenameDepartmentTeamCommandValidator : AbstractValidator<RenameDepartmentTeamCommand>
    {
        public RenameDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
