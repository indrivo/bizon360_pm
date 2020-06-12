using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.AddRemoveUserToProject
{
    public class ProjectAddOrRemoveUserCommand : IRequest
    {
        public Guid ProjectId { get; set; }

        public Guid ApplicationUserId { get; set; }

        /// <summary>
        /// If true the user should be included from the project
        /// If false the user should be excluded from the project
        /// </summary>
        public bool IncludeUser { get; set; }
    }
}
