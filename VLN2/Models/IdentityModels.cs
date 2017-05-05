using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace VLN2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public virtual ICollection<Project> Projects { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Displayname", this.Displayname.ToString()));

            return userIdentity;
        }

        public string Displayname { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public DbSet<Project> Projects { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.UserName).HasColumnName("Username");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).HasColumnName("ID");
            modelBuilder.Entity<CustomRole>().ToTable("UserRoles").Property(p => p.Id).HasColumnName("ID");

            modelBuilder.Entity<CustomUserRole>().ToTable("UserHasRole").Property(p => p.UserId).HasColumnName("UserID");
            modelBuilder.Entity<CustomUserRole>().Property(p => p.RoleId).HasColumnName("RoleID");

            modelBuilder.Entity<CustomUserClaim>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("ID");
            modelBuilder.Entity<CustomUserClaim>().Property(p => p.UserId).HasColumnName("UserID");

            modelBuilder.Entity<CustomUserLogin>().ToTable("UserLogins").Property(p => p.UserId).HasColumnName("UserID");

            modelBuilder.Entity<ApplicationUser>()
				 .HasMany(u => u.Projects)
				 .WithMany(l => l.ApplicationUsers)
				 .Map(ul =>
				 {
					 ul.MapLeftKey("ProjectID");
					 ul.MapRightKey("UserID");
					 ul.ToTable("UserHasProject");
				 });
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}