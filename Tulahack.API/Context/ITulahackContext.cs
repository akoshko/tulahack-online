using Microsoft.EntityFrameworkCore;
using Tulahack.Model;

namespace Tulahack.API.Context;

public interface ITulahackContext
{
    public DbSet<ContestApplication> ContestApplications { get; set; }
    public DbSet<PersonBase> Accounts { get; set; }
    public DbSet<Contestant> Contestants { get; set; }
    public DbSet<Expert> Experts { get; set; }
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ContestCase> ContestCases { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<StorageFile> StorageFiles { get; set; }

    public Task SaveChangesAsync();
    public Task SaveChangesAsync(CancellationToken cancellationToken);

    public Task<T> AddNewRecord<T>(T newItem) where T : class;
    public Task<T> UpdateRecord<T>(T record) where T : class;
    public Task<T> RemoveRecord<T>(T record) where T : class;
    public Task<T> AddNewRecord<T>(T newItem, CancellationToken cancellationToken) where T : class;
    public Task<T> UpdateRecord<T>(T record, CancellationToken cancellationToken) where T : class;
    public Task<T> RemoveRecord<T>(T record, CancellationToken cancellationToken) where T : class;
    public void ClearChangeTracker();
}