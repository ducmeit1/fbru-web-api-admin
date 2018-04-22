using FBru.Repository.Entities;
using System.Data.Entity;

namespace FBru.Repository
{
    public class FBruContext : DbContext
    {
        public FBruContext()
            : base("name=FBruConnection")
        {
        }

        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Dishes)
                .WithRequired(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Restaurant>()
                .HasMany(c => c.Dishes)
                .WithRequired(c => c.Restaurant)
                .HasForeignKey(c => c.RestaurantId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Dish>()
                .HasMany(c => c.Images)
                .WithRequired(c => c.Dish)
                .HasForeignKey(c => c.DishId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Dish>()
                .HasMany<Keyword>(c => c.Keywords)
                .WithMany(c => c.Dishes)
                .Map(c =>
                {
                    c.MapLeftKey("DishId");
                    c.MapRightKey("KeywordId");
                    c.ToTable("DishKeyword");
                });

            modelBuilder.Entity<UserGroup>()
                .HasMany(u => u.Users)
                .WithRequired(u => u.UserGroup)
                .HasForeignKey(u => u.GroupId)
                .WillCascadeOnDelete();
        }
    }
}