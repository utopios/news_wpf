```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0

dotnet ef dbcontext scaffold \
"Server=localhost;Port=3306;Database=demo_db;User=demo_user;Password=demo_pwd;" \
Pomelo.EntityFrameworkCore.MySql \
--output-dir Models \
--context DemoContext \
--force

```

### Models 

```c#
// Models/Customer.cs
using System.ComponentModel.DataAnnotations;

namespace EFCore9Workshop.Models;

public class Customer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Relations
    public Address? Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public CustomerProfile? Profile { get; set; }
    
    // Propriété calculée (non mappée en DB)
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
    
    public string FullName => $"{FirstName} {LastName}";
}

// Models/Address.cs
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    
    // Relation 1-1 avec Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

// Models/Order.cs
public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Relation Many-to-One avec Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    // Relation Many-to-Many avec Products
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
    // Relation avec Category
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

// Models/OrderItem.cs (Table de liaison Many-to-Many)
public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    // Propriété calculée
    public decimal LineTotal => Quantity * UnitPrice;
}

// Models/Category.cs
public class Category
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    // Auto-référence pour hiérarchie
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

// Models/CustomerProfile.cs (Relation 1-1)
public class CustomerProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}

// Models/Enums.cs
public enum OrderStatus
{
    Pending = 0,
    Processing = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}


// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using EFCore9Workshop.Models;

namespace EFCore9Workshop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            // Relation 1-1 avec Address
            entity.HasOne(c => c.Address)
                  .WithOne(a => a.Customer)
                  .HasForeignKey<Address>(a => a.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Relation 1-1 avec CustomerProfile
            entity.HasOne(c => c.Profile)
                  .WithOne(p => p.Customer)
                  .HasForeignKey<CustomerProfile>(p => p.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Relation 1-Many avec Orders
            entity.HasMany(c => c.Orders)
                  .WithOne(o => o.Customer)
                  .HasForeignKey(o => o.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            // Propriétés calculées ignorées
            entity.Ignore(c => c.Age);
            entity.Ignore(c => c.FullName);
        });

        // Configuration Address
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");
            entity.Property(e => e.Street).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
        });

        // Configuration Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            
            // Relation avec OrderItems
            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            
            // Relation avec Category
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuration Category (auto-référence)
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            
            entity.HasOne(c => c.ParentCategory)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuration OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            
            entity.Ignore(oi => oi.LineTotal);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Électronique", Description = "Produits électroniques" },
            new Category { Id = 2, Name = "Vêtements", Description = "Vêtements et accessoires" },
            new Category { Id = 3, Name = "Livres", Description = "Livres et magazines" },
            new Category { Id = 4, Name = "Smartphones", ParentCategoryId = 1, Description = "Téléphones intelligents" }
        );

        // Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "iPhone 15 Pro", Description = "Dernier iPhone", Price = 1199.99m, StockQuantity = 50, CategoryId = 4 },
            new Product { Id = 2, Name = "Samsung Galaxy S24", Description = "Dernier Samsung", Price = 999.99m, StockQuantity = 75, CategoryId = 4 },
            new Product { Id = 3, Name = "T-Shirt Blanc", Description = "T-shirt coton", Price = 19.99m, StockQuantity = 200, CategoryId = 2 },
            new Product { Id = 4, Name = "Clean Code", Description = "Livre de Robert Martin", Price = 39.99m, StockQuantity = 30, CategoryId = 3 }
        );

        // Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer 
            { 
                Id = 1, 
                FirstName = "Jean", 
                LastName = "Dupont", 
                Email = "jean.dupont@email.com", 
                DateOfBirth = new DateTime(1990, 5, 15),
                CreatedAt = DateTime.Now
            },
            new Customer 
            { 
                Id = 2, 
                FirstName = "Marie", 
                LastName = "Martin", 
                Email = "marie.martin@email.com", 
                DateOfBirth = new DateTime(1985, 8, 22),
                CreatedAt = DateTime.Now
            }
        );
    }

    // Override SaveChanges pour auditing automatique
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Customer && 
                   (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var customer = (Customer)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                customer.CreatedAt = DateTime.Now;
            }
            
            if (entry.State == EntityState.Modified)
            {
                customer.UpdatedAt = DateTime.Now;
            }
        }
    }
}

```

#### Application

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using EFCore9Workshop.Data;
using EFCore9Workshop.Services;
using EFCore9Workshop.ViewModels;

namespace EFCore9Workshop;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Configuration DbContext
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
                    
                    // Logging SQL en développement
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });

                // Services
                services.AddTransient<ICustomerService, CustomerService>();
                services.AddTransient<IOrderService, OrderService>();
                services.AddTransient<IProductService, ProductService>();

                // ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<CustomerViewModel>();
                services.AddTransient<OrderViewModel>();
                services.AddTransient<ProductViewModel>();

                // Windows
                services.AddSingleton<MainWindow>();
            })
            .Build();

        // Créer/Migrer la base de données au démarrage
        using (var scope = AppHost.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        AppHost?.Dispose();
        base.OnExit(e);
    }
}
```