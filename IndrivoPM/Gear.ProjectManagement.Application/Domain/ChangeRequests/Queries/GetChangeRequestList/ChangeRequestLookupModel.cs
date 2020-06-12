using System;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList
{
    public class ChangeRequestLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ActivityPriority Priority { get; set; }

        public ChangeRequestStatus Status { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public DateTime Date { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }
        public int Number { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public static Expression<Func<ChangeRequest, ChangeRequestLookupModel>> Projection
        {
            get
            {
                return changeRequest => new ChangeRequestLookupModel
                {
                    Id = changeRequest.Id,
                    Name = changeRequest.Name,
                    Description = changeRequest.Description,
                    AuthorId = changeRequest.CreatedBy,
                    Priority = changeRequest.Priority,
                    Date = changeRequest.ModifyTime,
                    Status = changeRequest.Status,
                    ProjectId = changeRequest.ProjectId,
                    ProjectName = changeRequest.Project.Name,
                    Number = changeRequest.Number,
                    CreatedTime = changeRequest.CreatedTime,
                    ModifyTime = changeRequest.ModifyTime
                };
            }
        }

        public static ChangeRequestLookupModel Create(ChangeRequest changeRequest, ApplicationUser user)
        {
            var projection = Projection.Compile().Invoke(changeRequest);

            projection.AuthorName = $"{user.FirstName} {user.LastName}";

            return projection;
        }
    }
}
