using System.Threading.Tasks;
using Gear.Common.Extensions.Result;
using MediatR;

namespace Gear.Manager.Core.Infrastructure.MediatorResultHandler
{
    public interface IMediatrResultFactory
    {
        Task<Result> Execute<T>(T request) where T : IRequest;
    }
}
