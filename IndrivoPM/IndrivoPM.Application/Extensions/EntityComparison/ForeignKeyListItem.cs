using System;

namespace Gear.Manager.Core.Extensions.EntityComparison
{
    public class ForeignKeyListItem
    {
        public Type Model { get; set; }

        public string FkeyName { get; set; }

        public string TableName { get; set; }
    }
}