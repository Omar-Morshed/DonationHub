using System;
using DonationHub.Application.Interfaces;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Implementation;
//? Why using T : Class , if there is a class that is not an Entity but is a class ?
//* By making a `BaseEntity`

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync([Id], cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
