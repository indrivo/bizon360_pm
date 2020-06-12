using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList
{
    public class UpdateActivityListCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Activity List")] public ActivityListStatus ActivityListStatus { get; set; }

        public Guid? SprintId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Description { get; set; }

        public string ProjectName { get; set; }

        public int ProjectNumber { get; set; }

        public Guid ProjectId { get; set; }

        public int ActivityListNumber { get; set; }

        public IFormFile File { get; set; }

        private static Expression<Func<ActivityListDetailModel, UpdateActivityListCommand>> Projection
        {
            get
            {
                return activityList => new UpdateActivityListCommand
                {
                    Id = activityList.Id,
                    ProjectId = activityList.ProjectId,
                    ProjectName = activityList.ProjectName,
                    ActivityListStatus = activityList.ActivityListStatus,
                    Description = activityList.Description,
                    StartDate = activityList.StartDate,
                    DueDate = activityList.DueDate,
                    Name = activityList.Name,
                    SprintId = activityList.SprintId,
                    ActivityListNumber = activityList.Number
                };
            }
        }

        public static UpdateActivityListCommand Create(ActivityListDetailModel model)
        {
            return Projection.Compile().Invoke(model);
        }
    }
}
