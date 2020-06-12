using System;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.UpdateChangeRequest
{
    public class UpdateChangeRequestCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Project")]
        public Guid ProjectId { get; set; }

        public ActivityPriority Priority { get; set; }

        public ChangeRequestStatus Status { get; set; }
        public IFormFile File { get; set; }
        public string ProjectName { get; set; }

        public int ProjectNumber { get; set; }

        public int ChangeRequestNumber { get; set; }
    }
}
