using System;
using DonationHub.Application.Interfaces;
using DonationHub.Infrastructure.Identity;
using DonationHub.Infrastructure.Implementation;
using DonationHub.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

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

        services.AddIdentityCore<ApplicationUser>(
            options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            }
        )
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<SoftDeleteInterceptor>();
    }
}
