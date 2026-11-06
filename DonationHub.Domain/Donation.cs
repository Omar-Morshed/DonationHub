using System;
using DonationHub.Domain.Enums;
using DonationHub.Domain.Interfaces;

namespace DonationHub.Domain;

public class Donation : ISoftDeletable
{
    public Guid Id { get; private set; }
    public Guid CampaignId { get; private set; }
    public Campaign Campaign { get; set; } = null!;
    public Guid DonorUserId { get; private set; }
    public ApplicationUser User { get; set; } = null!;
    public decimal Amount { get; private set; }
    public DonationStatus Status { get; private set; }
    public string FailureReason { get; private set; } = ""; //* Is the human readable failure message
    public string? ReceiptNumber { get; private set; }
    public string ReceiptFileName { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
