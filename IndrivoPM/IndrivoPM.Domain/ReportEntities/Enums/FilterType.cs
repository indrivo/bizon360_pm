namespace Gear.Domain.ReportEntities.Enums
{
    /// <summary>
    /// The FilterType.Guid means all options with Ids i.e. ProjectIds, ActivityIds etc.
    /// </summary>
    public enum FilterType
    {
        Guid,
        StartDate,
        DueDate,
        ActivityStatus,
        ProjectStatus,
        ProjectGroupIds,
        ProjectIds,
        ActivityListIds,
        SprintIds,
        ActivityIds,
        ActivityTypeIds,
        EmployeeIds
    }
}
