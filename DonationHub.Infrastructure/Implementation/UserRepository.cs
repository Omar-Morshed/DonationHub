using DonationHub.Application.Interfaces;
using DonationHub.Domain;

namespace DonationHub.Infrastructure.Implementation;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {}
}
