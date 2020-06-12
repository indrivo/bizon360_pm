using System.Collections.Generic;

namespace Gear.Domain.AppEntities
{
    public class Platform
    {
        public ICollection<ApplicationRole> Roles { get; set; }

        public string Name { get; set; }

        public bool Active { get; private set; }


        public virtual void Deactivate()
        {
            Active = false;
        }


        public virtual void Activate()
        {
            Active = true;
        }
    }
}
