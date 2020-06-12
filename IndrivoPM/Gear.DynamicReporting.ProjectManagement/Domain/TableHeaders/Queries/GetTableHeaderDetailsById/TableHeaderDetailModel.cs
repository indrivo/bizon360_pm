using System;
using System.Linq.Expressions;
using Gear.Domain.ReportEntities;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById
{
    public class TableHeaderDetailModel : IEquatable<TableHeaderDetailModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public bool Deletable { get; set; }

        public static Expression<Func<TableHeader<Guid>, TableHeaderDetailModel>> Projection
        {
            get
            {
                return tableHeader => new TableHeaderDetailModel
                {
                    Id = tableHeader.Id,
                    Active = tableHeader.Active,
                    Deletable = tableHeader.Deletable,
                    Name = tableHeader.Name,
                    Order = tableHeader.Order,
                    Width = tableHeader.Width
                };
            }
        }

        public static TableHeaderDetailModel Create(TableHeader<Guid> tableHeader)
        {
            return Projection.Compile().Invoke(tableHeader);
        }

        public static Expression<Func<ReportTableHeader<Guid>, TableHeaderDetailModel>> ReportTableHelperProjection
        {
            get
            {
                return reportTableHeader => new TableHeaderDetailModel
                {
                    Id = reportTableHeader.TableHeader.Id,
                    Active = reportTableHeader.TableHeader.Active,
                    Deletable = reportTableHeader.TableHeader.Deletable,
                    Name = reportTableHeader.TableHeader.Name,
                    Order = reportTableHeader.TableHeader.Order,
                    Width = reportTableHeader.TableHeader.Width
                };
            }
        }

        public static TableHeaderDetailModel Create(ReportTableHeader<Guid> reportTableHeader)
        {
            return ReportTableHelperProjection.Compile().Invoke(reportTableHeader);
        }

        public bool Equals(TableHeaderDetailModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TableHeaderDetailModel) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
