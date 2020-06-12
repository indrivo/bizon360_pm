using System;
using System.ComponentModel.DataAnnotations;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityListStatus
{
    public class UpdateActivityListStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        [Display(Name="Activity List Status")]
        public ActivityListStatus ActivityListStatus { get; set; }
    }
}
