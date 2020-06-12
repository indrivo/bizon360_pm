using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.CreateRecruitingPipeline
{
    public class CreateRecruitingPipelineCommand : IRequest
    {
        [Display(Name = "Recruiting pipeline name", Prompt = "Ex. ")]
        public string Name { get; set; }

        [Display(Name = "Description", Prompt = "same description")]
        public string Description { get; set; }
    }
}
