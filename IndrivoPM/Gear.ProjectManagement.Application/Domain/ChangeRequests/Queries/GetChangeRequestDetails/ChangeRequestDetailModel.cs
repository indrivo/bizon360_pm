using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails
{
    public class ChangeRequestDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ActivityPriority Priority { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorJobPosition { get; set; }

        public ChangeRequestStatus Status { get; set; }
        public int Number { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifyTime { get;  set; }
        public string Comment { get; set; }
        public Guid ReviewById { get; set; }
        public string ReviewByName { get; set; }
        public DateTime ReviewDate { get; set; }

        public int ProjectNumber { get; set; }

        public static Expression<Func<ChangeRequest, ChangeRequestDetailModel>> Projection
        {
            get
            {
                return changeRequest => new ChangeRequestDetailModel
                {
                     Id = changeRequest.Id,
                     Name = changeRequest.Name,
                     Description = changeRequest.Description,
                     ProjectId = changeRequest.ProjectId,
                     ProjectName = changeRequest.Project.Name,
                     Priority = changeRequest.Priority,
                     Status = changeRequest.Status,
                     CreatedBy = changeRequest.CreatedBy,
                     Date = changeRequest.ModifyTime,
                     Number = changeRequest.Number,
                     CreatedTime = changeRequest.CreatedTime,
                     ModifyTime = changeRequest.ModifyTime,
                     Comment = changeRequest.Comment,
                     ReviewById = changeRequest.ReviewBy,
                     ReviewDate = changeRequest.ReviewDate,
                     ProjectNumber = changeRequest.Project.Number ?? 0
                };
            }
        }

        public static ChangeRequestDetailModel Create(ChangeRequest changeRequest)
        {
            return Projection.Compile().Invoke(changeRequest);
        }
    }
}
