using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundTasks;

public class MyHangfireService(IServiceScopeFactory serviceScopeFactory, ILogger<MyHangfireService> logger)
{
    public async Task EveryMinuteProductCounter()
    {
        logger.LogInformation($"My hangfire service is started at {DateTimeOffset.Now}:");
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        var productCount = await dbContext.Products.CountAsync();
        logger.LogInformation($"Total count of products: {productCount}");
    }
}
