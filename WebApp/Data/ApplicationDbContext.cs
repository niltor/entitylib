using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entity;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<Entity.Entity> Entities { get; set; }
        public DbSet<Lib> Libs { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(e =>
            {
                //e.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });
            builder.Entity<Lib>(e =>
            {
                e.HasIndex(l => l.Language);
                e.HasIndex(l => l.Namespace);

            });
            builder.Entity<Entity.Entity>(e =>
            {
                e.HasIndex(e => e.Name);
            });

        }
    }
}
