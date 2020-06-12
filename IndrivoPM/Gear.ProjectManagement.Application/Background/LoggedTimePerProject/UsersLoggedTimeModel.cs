using System.Collections.Generic;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerProject
{
    public class UsersLoggedTimeModel
    {
        public Project Project { get; set; }

        public ICollection<UsersTimeModel> UsersTime { get; set; }
    }

    public class UsersTimeModel
    {
        public float Time { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
