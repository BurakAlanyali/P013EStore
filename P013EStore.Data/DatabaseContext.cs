using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Configurations;
using System.Reflection;

namespace P013EStore.Data
{
    public class DatabaseContext : DbContext
    {
        // Katmanlı mimaride bir projeden başka bir proje içindeki şeylere erişmek için : dependencies'ten add project reference diyip sonra seçmek istediğimiz projeyi işaretleyip ekliyoruz. Böylece o projedeki entities lere erişim sağlanmış oluyor.
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; database=P013EStore; Trusted_Connection=True");
            //optionsBuilder.UseSqlServer(@"Server=CanlıServerAdı; database=CanlıdakiDatabase; Username=CanlıVeritabanıKullanıcıAdı; Password=CanlıVeritabanıŞifre");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI ile veritabanı tablolarımız oluşurken veri tiplerini db kurallarını burada tanımlayabiliriz.
            modelBuilder.Entity<AppUser>().Property(a => a.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);//FluentAPI ile AppUser class ının Name Property si için oluşacak veritabanı kolonu ayarlarını bu şekilde belirleyebiliyoruz.
            modelBuilder.Entity<AppUser>().Property(a => a.Surname).HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.UserName).HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Password).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            modelBuilder.Entity<AppUser>().Property(a => a.Email).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Phone).HasMaxLength(20);

            // FluentAPI HasData ile db oluştuktan sonra başlangıç kayıtları ekleyebiliriz.

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = 1,
                Email = "info@P013EStore.com",
                Password = "123",
                UserName = "Admin",
                IsActive = true,
                IsAdmin = true,
                Name = "Admin",
                UserGuid = Guid.NewGuid(),// kullanıcıya benzersiz bir id no oluştur.
            });
            //modelBuilder.ApplyConfiguration(new BrandConfigurations()); // Brand için oluşturduğumuz config leri uyguladık.
            //modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // uygulamadaki tüm configurations class larını burada çalıştır.

            // Fluent Validation : data annotationdaki hata mesajları vb işlemlerini yöneteileceğimiz 3. parti paket.

            // Katmanlı mimaride MVCwebUI katmanından direk data katmanına erişilmesi istenmez, arada bir iş katmanının tüm db süreçlerini yönetmesi istenir. Bu yüzden solution a service adında yeni bir katman ekleyip Mvc katmanından service katmanına erişim vermemiz gerekir. Service katmanı da data katmanına erişir. Data katmanı da core katmanına erişir, böylece MVCUI > Service > Data > Core ile en üstten en alt katmana kadar ulaşılabilmiş olunur. 


            base.OnModelCreating(modelBuilder);
        }
    }
}