using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Api.Models;

namespace User.Api.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(c => c.Id);

            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(c => new { c.AppUserId, c.Value, c.Key });
            modelBuilder.Entity<UserProperty>().Property(c => c.Value).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(c => c.Key).HasMaxLength(100);

            modelBuilder.Entity<UserTag>()
                .ToTable("userTags")
                .HasKey(c => new { c.UserId, c.Tag });
            modelBuilder.Entity<UserTag>().Property(c => c.Tag).HasMaxLength(100);

            modelBuilder.Entity<BPFile>()
                .ToTable("UserBPFiles")
                .HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<UserProperty> UserProperties { get; set; }
    }
}
