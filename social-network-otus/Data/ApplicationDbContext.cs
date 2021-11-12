using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using social_network_otus.Data.Models;

namespace social_network_otus.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ApplicationUserFriend> UserFriends { get; set; }
        public ChatMessage ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(typeBuilder =>
            {
                typeBuilder
                    .Property(p => p.Id)
                    .HasMaxLength(255);

                typeBuilder
                    .Property(p => p.Email)
                    .HasMaxLength(250);
                typeBuilder
                    .Property(p => p.NormalizedEmail)
                    .HasMaxLength(250);

                typeBuilder
                    .Property(p => p.UserName)
                    .HasMaxLength(250);

                typeBuilder
                    .Property(p => p.NormalizedUserName)
                    .HasMaxLength(250);

            });

            builder.Entity<IdentityRole>(typeBuilder =>
            {
                typeBuilder
                    .Property(p => p.Id)
                    .HasMaxLength(255);

                typeBuilder
                    .Property(p => p.Name)
                    .HasMaxLength(250);

                typeBuilder
                    .Property(p => p.NormalizedName)
                    .HasMaxLength(250);

            });

            builder
                .Entity<ApplicationUser>()
                .HasMany<Chat>()
                .WithOne(chat => chat.User);

            builder.Entity<ApplicationUserFriend>()
                .HasOne(pt => pt.Friend)
                .WithMany(t => t.Friends)
                .HasForeignKey(pt => pt.FriendId)
                .HasPrincipalKey(p => p.Id);

            builder
                .Entity<ApplicationUser>()
                .HasMany<Chat>()
                .WithOne(chat => chat.AnotherUser);

        }
    }
}
