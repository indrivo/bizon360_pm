using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.SwapCheckItems
{
    public class SwapCheckItemsCommand : IRequest
    {
        public IList<Guid> Ids { get; set; }
    }
}
