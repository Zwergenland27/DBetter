using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class DatabaseTrainRunScrapingScheduler(DBetterContext database) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(DatabaseTrainRunScrapingScheduler));
    
    public async Task Execute(IJobExecutionContext context)
    {
        var trainRunIdsToScrape = await database.Routes
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToListAsync();
        
        TrainRunScraperJob.AddTrainRuns(trainRunIdsToScrape);
    }
}