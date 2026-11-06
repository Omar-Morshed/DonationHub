using System;
using DonationHub.Domain.Enums;
using DonationHub.Domain.Interfaces;

namespace DonationHub.Domain;

public class Category : ISoftDeletable
{
    public Guid Id { get; private set; }
    public string NameAr { get; private set; } = "";
    public string NameEn { get; private set; } = "";
    public string DescriptionAr { get; private set; } = "";
    public string DescriptionEn { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public IReadOnlyList<Campaign> Campaigns { get; set; } = new List<Campaign>();
}
