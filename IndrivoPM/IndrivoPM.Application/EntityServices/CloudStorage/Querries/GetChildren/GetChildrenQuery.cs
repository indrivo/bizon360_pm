using System.Collections.Generic;
using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren
{
    public class GetChildrenQuery : IRequest<List<CloudMetaData>>
    {
        public string FilePath { get; set; }

        public ExternalProviders ExternalProvider { get; set; }
    }
}
