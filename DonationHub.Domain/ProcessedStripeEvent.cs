using System;

namespace DonationHub.Domain;

public class ProcessedStripeEvent
{
    public string EventId { get; private set; }
    public string Type { get; private set; }
    public DateTime ProcessedAt { get; set; }
}
