using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentDetails
{
    public class GetMainCommentDetailQueryValidator : AbstractValidator<MainCommentDetailModel>
    {
        public GetMainCommentDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
