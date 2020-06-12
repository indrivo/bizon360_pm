using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeListByProject
{
    public class GetLoggedTimeListByProjectQueryValidator : AbstractValidator<GetLoggedTimeListByProjectQuery>
    {
        public GetLoggedTimeListByProjectQueryValidator()
        {
            RuleFor(lt => lt.ProjectId).NotNull().NotEmpty();
        }
    }
}
