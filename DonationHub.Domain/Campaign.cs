using System;
using System.ComponentModel.Design;
using System.Security.AccessControl;
using DonationHub.Domain.Enums;

namespace DonationHub.Domain;

public class Campaign
{
    public Guid Id { get; private set; }
    public string TitleAr { get; private set; } = "";
    public string TitleEn { get; private set; } = "";
    public string DescriptionAr { get; private set; } = "";
    public string DescriptionEn { get; private set; } = "";
    public decimal GoalAmount { get; private set; }
    public decimal RaisedAmount { get; private set; }
    public DateTime EndDate { get; private set; }
    public Guid CategoryId { get; private set; }
    public string? BannerFileName { get; private set; }
    public CampaignStatus Status { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public byte[] RowVersion { get; private set; } // Concurrency Token توكن التزامن , used to avoid Race Condition

    public static Campaign Create(
        string titleAr,
        string titleEn,
        string descriptionAr,
        string descriptionEn,
        decimal goalAmount,
        DateTime endDate,
        Guid categoryId,
        Guid createdByUserId,
        DateTime utcNow,
        Guid? id = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titleAr);
        ArgumentException.ThrowIfNullOrWhiteSpace(titleEn);
        ArgumentException.ThrowIfNullOrWhiteSpace(descriptionAr);
        ArgumentException.ThrowIfNullOrWhiteSpace(descriptionEn);

        if (goalAmount <= 0)
            throw new ArgumentException("Goal amount must be greater than zero.", nameof(goalAmount));

        if (endDate <= utcNow)
            throw new ArgumentException("End date must be in the future.", nameof(endDate));

        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID is required.", nameof(categoryId));

        if (createdByUserId == Guid.Empty)
            throw new ArgumentException("Creator User ID is required.", nameof(createdByUserId));

        return new Campaign
        {
            Id = id ?? Guid.NewGuid(),
            TitleAr = titleAr.Trim(),
            TitleEn = titleEn.Trim(),
            DescriptionAr = descriptionAr.Trim(),
            DescriptionEn = descriptionEn.Trim(),
            GoalAmount = goalAmount,
            RaisedAmount = 0,
            EndDate = endDate,
            CategoryId = categoryId,
            CreatedByUserId = createdByUserId,
            Status = CampaignStatus.Active,
            CreatedAt = utcNow,
            IsDeleted = false,
            RowVersion = []
        };
    }
    public bool SetGoalAmount(decimal GoalAmount)
    {
        if (GoalAmount > 0 && GoalAmount >= RaisedAmount)
        {
            this.GoalAmount = GoalAmount;
            return true;
        }
        return false;
    }

    public bool SetEndDate(DateTime EndDate, DateTime UtcNow)
    {
        if (UtcNow < EndDate)
        {
            this.EndDate = EndDate;
            this.UpdatedAt = UtcNow;
            return true;
        }
        return false;
    }

    public bool IncreaseRaisedAmount(decimal amount, DateTime UtcNow)
    {
        if (amount <= 0 || Status != CampaignStatus.Active || UtcNow > EndDate)
            return false;

        RaisedAmount += amount;
        if (RaisedAmount >= GoalAmount)
        {
            // close the campaign
            Status = CampaignStatus.Completed;
        }
        return true;
    }
}
