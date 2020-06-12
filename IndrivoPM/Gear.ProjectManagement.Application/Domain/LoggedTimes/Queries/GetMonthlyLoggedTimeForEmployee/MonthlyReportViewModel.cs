namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLoggedTimeForEmployee
{
    public class MonthlyReportViewModel
    {
        public int NumberOfDays { get; set; }

        public string[] DaysOfWeek { get; set; }

        public float[] TimeLogs { get; set; }

        public float TotalPerMonth { get; set; }

        public float TotalEstimated { get; set; }
    }
}
