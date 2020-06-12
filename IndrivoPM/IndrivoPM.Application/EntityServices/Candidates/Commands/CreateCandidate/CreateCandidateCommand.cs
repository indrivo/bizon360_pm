using System;
using System.ComponentModel.DataAnnotations;
using Gear.Domain.HrmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.CreateCandidate
{
    public class CreateCandidateCommand : IRequest
    {
        [Display(Name = "First Name", Prompt = "Ex. John")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name", Prompt = "Ex. Doe")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number", Prompt = "Ex. 079123456")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email", Prompt = "Ex. john.doe@gmail.com")]
        public string Email { get; set; }

        [Display(Name = "Recruitment Stage", Prompt = "-Select-")]
        public Guid? RecruitmentStageId { get; set; }

        [Display(Name = "Desired Salary", Prompt = "Ex. 2000")]
        public decimal? DesiredSalary { get; set; }

        [Display(Name = "Additional info")]
        public string Description { get; set; }

        [Display(Name = "Job position")]
        public Guid JobPositionId { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus CandidateStatus { get; set; } = CandidateStatus.New;
    }
}
