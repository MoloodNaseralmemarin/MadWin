using MadWin.Application.Repositories;
using MadWin.Application.Services;
using MadWin.Core.Convertors;
using MadWin.Core.Interfaces;
using MadWin.Core.Settings;
using MadWin.Infrastructure.Context;
using MadWin.Infrastructure.Repositories;
using MadWin.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Shop2City.Core.Services.Permissions;
using Shop2City.Core.Services.ProductImages;
using Shop2City.Core.Services.Products;
using Shop2City.Core.Services.Properties;
using Shop2City.Core.Services.PropertyTechnicalProducts;
using Shop2City.Core.Services.PropertyTechnicals;
using Shop2City.Core.Services.PropertyTitles;
using Shop2City.Core.Services.SlideShows;
using Shop2City.Core.Services.Transactions;
using Shop2City.Core.Services.UserPanel;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);
// ────── Configuration ──────
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// ────── Logging (Serilog) ──────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("Development"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: new ColumnOptions
        {
            Store = new Collection<StandardColumn>
            {
                StandardColumn.Message,
                StandardColumn.Level,
                StandardColumn.TimeStamp,
                StandardColumn.Exception
            }
        }
    )
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// ────── Database Context ──────
builder.Services.AddDbContext<MadWinDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Production");
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new ArgumentNullException(nameof(connectionString), "Connection string is null or empty.");
    options.UseSqlServer(connectionString);
});

// ────── Session & Caching ──────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // فقط وقتی https بود Secure میشه
    options.Cookie.SameSite = SameSiteMode.Lax; // معمولا کفایت می‌کنه
});

// ────── Authentication ──────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

// ────── Dependency Injection ──────
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<INumberRoundingService, NumberRoundingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderWidthPartRepository, OrderWidthPartRepository>();
builder.Services.AddScoped<ICurtainComponentProductGroupRepository, CurtainComponentProductGroupRepository>();
builder.Services.AddScoped<ICurtainComponentDetailService, CurtainComponentDetailService>();
builder.Services.AddScoped<ICurtainComponentDetailRepository, CurtainComponentDetailRepository>();
builder.Services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
builder.Services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IFactorRepository, FactorRepository>();
builder.Services.AddScoped<IFactorService, FactorService>();
builder.Services.AddScoped<IFactorDetailRepository, FactorDetailRepository>();
builder.Services.AddScoped<IFactorDetailService, FactorDetailService>();
builder.Services.AddScoped<IUserDiscountCodeRepository, UserDiscountCodeRepository>();
builder.Services.AddScoped<IUserPanelService, UserPanelService>();
builder.Services.AddScoped<IViewRenderService, RenderViewToString>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISlideShowService, SlideShowService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IPropertyTechnicalProductService, PropertyTechnicalProductService>();
builder.Services.AddScoped<ICommissionRateRepository, CommissionRateRepository>();
builder.Services.AddScoped<ICurtainComponentRepository, CurtainComponentRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyTitleService, PropertyTitleService>();
builder.Services.AddScoped<IPropertyTechnicalService, PropertyTechnicalService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ISmsRepository, SmsRepository>();
builder.Services.AddScoped<ISmsSenderService, SmsSenderService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// ────── Settings Bindings ──────
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));

// ────── CORS ──────
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCors", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ────── MVC & Razor ──────
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// ────── Build App ──────
var app = builder.Build();

// ────── Error Handling ──────
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ────── Middleware ──────
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("EnableCors");

app.UseSession();           // 👈 خیلی مهم: قبل از Auth و Authorization
app.UseAuthentication();
app.UseAuthorization();

// ────── Endpoints ──────
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ────── Run ──────
app.Run();
