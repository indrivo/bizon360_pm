using System;
using System.Threading.Tasks;
using Gear.Common.Extensions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gear.Manager.Core.Infrastructure.MediatorResultHandler
{
    public class MediatrResultFactory : IMediatrResultFactory
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<IMediatrResultFactory> _logger;

        public MediatrResultFactory(IMediator mediatr, ILogger<IMediatrResultFactory> logger)
        {
            _mediatr = mediatr;
            _logger = logger;
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns>Query execution status</returns>
        public async Task<Result> Execute<T>(T request) where T : IRequest
        {
            try
            {
                await _mediatr.Send(request);
                return Result.Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception has been occured has occured!");
                return Result.Fail(e.Message);
            }        
        }
    }
}
