using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Querries.GetCheckListForActivity
{
    public class GetCheckListForActivityQuery : IRequest<CheckListViewModel>
    {
        public Guid ActivityId { get; set; }
    }
}
