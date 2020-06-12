using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails
{
    public class GetChangeRequestDetailsQueryValidator : AbstractValidator<GetChangeRequestDetailsQuery>
    {
        public GetChangeRequestDetailsQueryValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
