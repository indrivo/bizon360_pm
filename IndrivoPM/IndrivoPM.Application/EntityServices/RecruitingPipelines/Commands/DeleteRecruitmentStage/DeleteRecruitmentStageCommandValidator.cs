using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitmentStage
{
    public class DeleteRecruitmentStageCommandValidator : AbstractValidator<DeleteRecruitmentStageCommand>
    {
        public DeleteRecruitmentStageCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
