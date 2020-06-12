namespace Gear.Common.Extensions.DateTimeExtensions
{
    public static class DateTimeExtensions
    {
        public static PriorityByDate GetPriorityByDate(this System.DateTime? date)
        {
            if (date.HasValue)
            {
                if ((date.Value.Date - System.DateTime.Today.Date).Days <= 14)
                    return PriorityByDate.Critical;
                else if ((date.Value.Date - System.DateTime.Today.Date).Days <= 30)
                    return PriorityByDate.High;
                else if ((date.Value.Date - System.DateTime.Today.Date).Days <= 60)
                    return PriorityByDate.Medium;
                else return PriorityByDate.Low;
            }

            return PriorityByDate.Low;
        }

        public static bool IsInRangeInclusive(this System.DateTime date, System.DateTime start, System.DateTime end)
        {
            return date >= start && date <= end;
        }

        public static string[] GetDays(this System.DateTime date,  System.DateTime? endDate)
        {
            var days = endDate.HasValue ? DaysInPeriod(date, endDate) : DaysInMonth(date);

            var array = new string[days];
            for (var i = 0; i < days; i++)
            {
                array[i] = $"{date.AddDays(i).Day:D2} {date.AddDays(i).DayOfWeek.ToString()[0]}";
            }

            return array;
        }

        public static bool IsWeekend(this System.DateTime date)
        {
            return (date.DayOfWeek == System.DayOfWeek.Sunday || date.DayOfWeek == System.DayOfWeek.Saturday);
        }

        private static int DaysInMonth(System.DateTime date)
        {
            return System.DateTime.DaysInMonth(date.Year, date.Month);
        }

        private static int DaysInPeriod(System.DateTime date, System.DateTime? endDate)
        {
            return (endDate.Value - date).Days + 1;
        }
    }

    public enum PriorityByDate
    {
        Low,
        Medium,
        High,
        Critical
    }
}
