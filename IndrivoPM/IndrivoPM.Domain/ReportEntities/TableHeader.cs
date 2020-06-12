using System;
using System.Collections.Generic;

namespace Gear.Domain.ReportEntities
{
    public class TableHeader<TUserIdType>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public bool Deletable { get; set; }

        #region Navigation Properties
        public ICollection<ReportTableHeader<TUserIdType>> ReportTableHeaders { get; set; }
        #endregion
    }
}
