using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.SpentTimeReport
{
    public class ProjectSpentTimeHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private Timer _timer;

        private IServiceProvider Services { get; }

        public ProjectSpentTimeHostedService(ILogger<ProjectSpentTimeHostedService> logger,IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        /// <summary>
        /// Starts the hosted background task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Project Report Hosted Service is starting.");

            _timer = new Timer(CreateScope, null, TimeSpan.Zero,
                TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        private void CreateScope(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IProjectSpentTimeProcessingService>();

                scopedProcessingService.ExecuteProcess();
            }
        }

        /// <summary>
        /// Gracefully stop the background task when the app shuts down
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "SpentTime Hosted Service is gracefully stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
