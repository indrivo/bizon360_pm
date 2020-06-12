using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentDetails
{
    public class GetSubCommentDetailQueryValidator : AbstractValidator<SubCommentDetailModel>
    {
        public GetSubCommentDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
