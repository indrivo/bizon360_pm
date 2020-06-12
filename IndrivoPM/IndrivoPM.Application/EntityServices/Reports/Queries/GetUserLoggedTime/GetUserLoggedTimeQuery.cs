using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime
{
    public class GetUserLoggedTimeQuery : IRequest<UserLoggedTimeListViewModel>
    {
        public Guid EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
