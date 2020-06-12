using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.CreateActivityList
{
    public class CreateActivityListCommand : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        [Display(Name = "Activity List")]
        public ActivityListStatus ActivityListStatus { get; set; } = ActivityListStatus.New;

        [DisplayName("Sprint")]
        public Guid? SprintId { get; set; }

        [DisplayName("Start date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due date")]
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }

        public int ProjectNumber { get; set; }

        public IFormFile File { get; set; }
    }
}
