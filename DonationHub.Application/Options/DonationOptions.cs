using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DonationHub.Application.Options;

public class DonationOptions
{
    [Range(1.00, 100)]
    public decimal MinAmount { get; init; } = 1.00m;
    [Range(100, 10_000)]
    public decimal MaxAmount { get; init; } = 10_000.00m;
    public DateTime PendingTimeoutMinutes { get; init; } = DateTime.UtcNow.AddMinutes(30);
    public string DefaultCurrency { get; init; } = "USD";

}
