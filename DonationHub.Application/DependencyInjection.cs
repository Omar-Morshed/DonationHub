using System;
using DonationHub.Application.Options;
using FluentValidation;
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

        services.AddMediatR(options =>
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}
