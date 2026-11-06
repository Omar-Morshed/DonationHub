using System;
using DonationHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }
    public DbSet<PaymentAttempt> PaymentAttempts { get; set; }
    public DbSet<ProcessedStripeEvent> ProcessedStripeEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<IdempotencyRecord>().HasKey(ir => ir.Key);
        modelBuilder.Entity<ProcessedStripeEvent>().HasKey(pse => pse.EventId);
    }

}
