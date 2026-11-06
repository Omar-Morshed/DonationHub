using System;
using DonationHub.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DonationHub.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"))
                .AddInterceptors(interceptor);

            //? How to change the default tracker without writing `AsNoTracking` all over the code ? (to make it AsNoTracking)
            //* options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //? What is the difference between the `QueryTrackingBehavior.NoTracking` & `QueryTrackingBehavior.NoTrackingWithIdentityResolution` ?
            //* WithIdentityResolution if it is in the change tracker gets the data once .. while the NoTracking gets it on every call (when using many `.include()`)
        });
        
    }
}
