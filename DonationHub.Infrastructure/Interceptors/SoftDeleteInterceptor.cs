using System;
using DonationHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DonationHub.Infrastructure.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplySoftDelete(DbContext? context)
    {
        if(context is null)
            return;
        
        var entries = context.ChangeTracker.Entries<ISoftDeletable>();
        foreach(var entry in entries)
        {
            if(entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                // entity.Property(x => x.IsDeleted).CurrentValue = true;
                entry.Entity.IsDeleted = true;
                entry.Property(x => x.DeletedAtUtc).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
