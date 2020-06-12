using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSstp
{
    internal class GetProjectSstpQueryValidator : AbstractValidator<GetProjectSstpQuery>
    {
        public GetProjectSstpQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty().NotNull();
        }
    }
}
