using System;
using System.ComponentModel.DataAnnotations;
using Gear.Domain.HrmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidateStatus
{
    public class UpdateCandidateStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Status")]
        public CandidateStatus CandidateStatus { get; set; }
    }
}
