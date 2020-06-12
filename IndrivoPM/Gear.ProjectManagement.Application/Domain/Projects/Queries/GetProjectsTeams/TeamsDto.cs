using System;
using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsTeams
{
    public class TeamsDto
    {
        public ICollection<UserDto> Users { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
