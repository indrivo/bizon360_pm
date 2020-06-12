using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeList
{
    public class GetActivityTypeListQuery:IRequest<ActivityTypeListViewModel>
    {
    }
}
