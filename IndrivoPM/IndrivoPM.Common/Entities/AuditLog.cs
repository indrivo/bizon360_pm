using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Extensions.Result;

namespace Gear.Common.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid AuditUserId { get; set; }

        public string AuditUserName { get; set; }

        public string EntityPk { get; set; }

        public string TableName { get; set; }

        public string Title { get; set; }

        public System.DateTime AuditDate { get; set; }

        public string EntityTypeName { get; set; }

        public string AuditData { get; set; }

        public string Action { get; set; }

        private HashSet<EntryChange> _changes;

        public IEnumerable<EntryChange> Changes => _changes?.ToList();

        /// <summary>
        /// Called only once in the database context set-up.
        /// </summary>
        /// <param name="list"></param>
        public Result SetChanges(List<EntryChange> list)
        {
            if (_changes != null)
            {
                return Result.Fail("Cannot modify audit entry");
            }
            _changes = new HashSet<EntryChange>(list);

            return Result.Ok();
        }
    }

    public class AuditingActions
    {
        public const string CreateOperation = "Insert";

        public const string UpdateOperation = "Update";
    }
}
