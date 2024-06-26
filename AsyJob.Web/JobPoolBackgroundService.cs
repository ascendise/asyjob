﻿using AsyJob.Lib.Runner;

namespace AsyJob.Web
{
    /// <summary>
    /// Bit of a dummy service that is only responsible for making the DI container instantiate the job pool 
    /// at startup. 
    /// 
    /// Could later be refunctioned to a pool monitoring service, as long as it uses a JobPool 
    /// </summary>
    /// <param name="jobPool"></param>
    internal class JobPoolBackgroundService(IJobPool jobPool) : BackgroundService
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IJobPool _jobPool = jobPool;
#pragma warning restore IDE0052 // Remove unread private members

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Idle(stoppingToken);

        private static async Task Idle(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested) { }
            }, stoppingToken);
        }
    }
}
