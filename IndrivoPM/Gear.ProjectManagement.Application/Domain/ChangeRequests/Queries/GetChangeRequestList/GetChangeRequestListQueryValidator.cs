using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList
{
    public class GetChangeRequestListQueryValidator : AbstractValidator<GetChangeRequestListQuery>
    {
        public GetChangeRequestListQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();
        }
    }
}
