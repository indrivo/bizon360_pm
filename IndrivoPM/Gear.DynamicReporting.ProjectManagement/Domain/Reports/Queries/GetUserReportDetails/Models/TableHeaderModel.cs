namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails.Models
{
    public class TableHeaderModel
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public bool Deletable { get; set; }
    }
}
