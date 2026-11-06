using System;
using DonationHub.Application.Interfaces;
using DonationHub.Domain;

namespace DonationHub.Infrastructure.Implementation;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
