using Batura.Promoos.TestTask.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Batura.Promoos.TestTask.Model;

public class TaskDbContext : DbContext
{
    public TaskDbContext() : base() { }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>()
            .Property(p => p.Version)
            .IsRowVersion();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ((EntityBase)entry.Entity).CreatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
