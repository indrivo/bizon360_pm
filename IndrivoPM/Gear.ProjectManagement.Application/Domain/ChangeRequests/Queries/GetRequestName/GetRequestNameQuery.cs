using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetRequestName
{
    public class GetRequestNameQuery:IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
