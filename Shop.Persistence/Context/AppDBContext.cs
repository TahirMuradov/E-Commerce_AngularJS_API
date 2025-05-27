

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Domain.Entities;
using System.Text.Json;

namespace Shop.Persistence.Context
{
  public  class AppDBContext: IdentityDbContext<User,Role, Guid>
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



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Product>()
                .Property(p => p.ImageUrls)
                 .HasConversion(
    v => JsonConvert.SerializeObject(v),
    v => JsonConvert.DeserializeObject<List<string>>(v));

        }
    }
}
