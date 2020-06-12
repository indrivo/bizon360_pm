using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.UpdateRecruitingPipeline
{
    public class UpdateRecruitingPipelineCommand : IRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Recruiting pipeline name", Prompt = "Ex. ")]
        public string Name { get; set; }

        [Display(Name = "Description", Prompt = "same description")]
        public string Description { get; set; }
    }
}
