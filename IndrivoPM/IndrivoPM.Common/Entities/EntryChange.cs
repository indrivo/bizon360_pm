using System;

namespace Gear.Common.Entities
{
    public class EntryChange
    {
        [Obsolete("Used only for ef core and mappers")]
        public EntryChange()
        {
            
        }

        public EntryChange(string columnName, string originalValue, string newValue)
        {
            Id = Guid.NewGuid();
            ColumnName = columnName;
            OriginalValue = originalValue;
            NewValue = newValue;
        }

        public Guid Id { get; private set; }

        public AuditLog AuditLog { get; set; }

        public Guid AuditLogId { get; private set; }

        public string ColumnName { get; private set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }
    }
}