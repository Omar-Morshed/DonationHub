using System;
using DonationHub.Application.Options;
using Microsoft.Extensions.DependencyInjection;

namespace DonationHub.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration("JWT")
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
