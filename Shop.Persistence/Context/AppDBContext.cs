

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Domain.Entities;
using Shop.Domain.Entities.WebUIEntites;

namespace Shop.Persistence.Context
{
    public class AppDBContext : IdentityDbContext<User, Role, Guid>
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLanguage> ProductLanguages { get; set; }
        public DbSet<SizeProduct> SizeProducts { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<ShippingMethodLanguage> ShippingMethodLanguages { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodLanguages> PaymentMethodLanguages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<HomeSliderItem> HomeSliderItems { get; set; }
        public DbSet<HomeSliderLanguage> HomeSliderLanguages { get; set; }
        public DbSet<TopCategoryArea> TopCategoryAreas { get; set; }
        public DbSet<TopCategoryAreaLanguage> TopCategoryAreaLanguages { get; set; }
        public DbSet<DisCountArea> DisCountAreas { get; set; }
        public DbSet<DisCountAreaLanguage> DisCountAreaLanguages { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
       builder.Entity<HomeSliderItem>()
                .HasOne(h => h.Image)
                .WithOne(i => i.HomeSliderItem)
                .HasForeignKey<Image>(i => i.HomeSliderItemId);
            builder.Entity<TopCategoryArea>()
                .HasOne(t => t.Image)
                .WithOne(i => i.TopCategoryArea)
                .HasForeignKey<Image>(i => i.TopCategoryAreaId);
            //        builder.Entity<Product>()
            //            .Property(p => p.ImageUrls)
            //             .HasConversion(
            //v => JsonConvert.SerializeObject(v),
            //v => JsonConvert.DeserializeObject<List<string>>(v));


        }
    }
}
