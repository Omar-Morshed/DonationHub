using System;
using DonationHub.Domain.Enums;

namespace DonationHub.Domain;

public class Donation
{
    public Guid Id { get; private set; }
    public Guid CampaignId { get; private set; }
    public Guid DonorUserId { get; private set; }
    public decimal Amount { get; private set; }
    public DonationStatus Status { get; private set; }
    public string FailureReason { get; private set; } = ""; //* Is the human readable failure message
    public string? ReceiptNumber { get; private set; }
    public string ReceiptFileName { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; private set; }
}
