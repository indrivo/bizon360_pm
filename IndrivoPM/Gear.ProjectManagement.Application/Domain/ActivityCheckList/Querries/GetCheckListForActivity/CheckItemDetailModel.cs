using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Querries.GetCheckListForActivity
{
    public class CheckItemDetailModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = "";

        public float LoggedTime { get; set; }
        public Guid? LoggedTimeId { get; set; }
        /// <summary>
        /// UserName of the person who completed the item
        /// </summary>
        public string UserName { get; set; } = "";

        public bool IsCompleted { get; set; }

        public Guid ActivityId { get; set; }

        public string Tracker { get; set; }
        public DateTime DateOfWork { get; set; }
        public static Expression<Func<CheckItem, CheckItemDetailModel>> Projection
        {
            get
            {
                return checkItem => new CheckItemDetailModel
                {
                    Id = checkItem.Id,
                    Content = checkItem.Content,
                    IsCompleted = checkItem.IsCompleted,
                    LoggedTime = checkItem.LoggedTime != null ? checkItem.LoggedTime.Time : 0,
                    LoggedTimeId = checkItem.LoggedTimeId != Guid.Empty ? checkItem.LoggedTimeId : null,
                    UserName = checkItem.ApplicationUser.UserName,
                    ActivityId = checkItem.ActivityId,
                    Tracker = checkItem.LoggedTime != null ? checkItem.LoggedTime.Tracker.Name : string.Empty,
                    DateOfWork = checkItem.LoggedTime != null ? checkItem.LoggedTime.DateOfWork : DateTime.Now
                };
            }
        }

        public static CheckItemDetailModel Create(CheckItem checkItem)
        {
            return Projection.Compile().Invoke(checkItem);
        }
    }
}
