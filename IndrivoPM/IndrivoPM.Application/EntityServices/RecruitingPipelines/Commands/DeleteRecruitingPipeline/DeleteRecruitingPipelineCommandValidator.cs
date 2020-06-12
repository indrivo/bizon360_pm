using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitingPipeline
{
    public class DeleteRecruitingPipelineCommandValidator : AbstractValidator<DeleteRecruitingPipelineCommand>
    {
        public DeleteRecruitingPipelineCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
