using Kt12Profiles.Models;
using Microsoft.EntityFrameworkCore;

namespace Kt12Profiles.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasOne(user => user.Profile)
            .WithOne(profile => profile.User)
            .HasForeignKey<UserProfile>(profile => profile.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<User>().Navigation(user => user.Profile).IsRequired();
        builder.Entity<UserProfile>().Property(profile => profile.UserId).ValueGeneratedNever();
    }
}
