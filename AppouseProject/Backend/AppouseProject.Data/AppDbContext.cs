using AppouseProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Data
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<ImageFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ImageFile>().HasOne(x=>x.AppUser).WithMany(x => x.Files).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);

            #region // Sistem ilk migration yapılırken admin kullanıcını oluşturma
            var adminRole = new AppRole() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" };
            builder.Entity<AppRole>().HasData(adminRole);

            var hasher = new PasswordHasher<AppUser>();
            var adminUser = new AppUser
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123.")
            };
            builder.Entity<AppUser>().HasData(adminUser);

            var adminUserRole = new IdentityUserRole<int>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            };

            builder.Entity<IdentityUserRole<int>>().HasData(adminUserRole);

            builder.Entity<Quota>().HasData(
                new Quota()
                {
                    Id = adminUser.Id,
                    StorageSpaceByte = 104857600,
                    UsedSpaceByte=0
                });
            #endregion
        }
    }
}
