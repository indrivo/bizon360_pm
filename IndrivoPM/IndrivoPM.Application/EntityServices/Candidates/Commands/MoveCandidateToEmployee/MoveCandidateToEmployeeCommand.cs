using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToEmployee
{
    public class MoveCandidateToEmployeeCommand : IRequest
    {
        [Display(Name = "Candidate")]
        public Guid CandidateId { get; set; }
    }
}
