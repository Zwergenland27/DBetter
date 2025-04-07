using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class DatabaseRoutesScrapingScheduler(DBetterContext database) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(DatabaseRoutesScrapingScheduler));
    
    public async Task Execute(IJobExecutionContext context)
    {
        var routesToScrape = await database.Routes
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToListAsync();
        
        RouteScraperJob.AddRoutes(routesToScrape);
    }
}