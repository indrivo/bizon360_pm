using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.CreateChangeRequest
{
    public class CreateChangeRequestCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string Comment { get; set; }
        [DisplayName("Project")]
        public Guid ProjectId { get; set; }
        public ActivityPriority Priority { get; set; }
        public string ProjectName { get;set;}
        public Guid EntityId { get; set; }
        public IFormFile File { get; set; }

        public int ProjectNumber { get; set; }
    }
}
