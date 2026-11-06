using System;

namespace DonationHub.Domain;

public class Category
{
    public Guid Id { get; private set; }
    public string NameAr { get; private set; } = "";
    public string NameEn { get; private set; } = "";
    public string DescriptionAr { get; private set; } = "";
    public string DescriptionEn { get; private set; } = "";
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
