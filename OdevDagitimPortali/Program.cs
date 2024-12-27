using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using OdevDagitimPortali.Areas.Identity.Data;
using OdevDagitimPortali.Data;
using OdevDagitimPortali.Hubs;
using OdevDagitimPortali.Repository;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantı dizesi
var applicationDbContextConnection = builder.Configuration.GetConnectionString("ApplicationDbContext")
    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");
var odevDagitimDbContextConnection = builder.Configuration.GetConnectionString("OdevDagitimDbContext")
    ?? throw new InvalidOperationException("Connection string 'OdevDagitimDbContext' not found.");

// ApplicationDbContext için SQL Server bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(applicationDbContextConnection);
});

// OdevDagitimDbContext için SQL Server bağlantısı
builder.Services.AddDbContext<OdevDagitimDbContext>(options =>
{
    options.UseSqlServer(odevDagitimDbContextConnection);
});

// Identity işlemleri için OdevDagitimDbContext kullanımı
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireUppercase = false; // Şifre doğrulama kuralları
})
.AddEntityFrameworkStores<OdevDagitimDbContext>();
=======
using OdevDagitimPortali.Repository;
using Microsoft.AspNetCore.Identity;
using OdevDagitimPortali.Data;
using OdevDagitimPortali.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// AutoMapper Konfigürasyonu
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae

// Repository'leri Scoped olarak kaydet
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SubmissionRepository>();
builder.Services.AddScoped<AssignmentRepository>();

<<<<<<< HEAD
=======
// ApplicationDbContext için SQL Server bağlantısı (Veritabanı işlemleri için)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext"));
});

// Identity işlemleri için OdevDagitimDbContext
builder.Services.AddDbContext<OdevDagitimDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OdevDagitimDbContext"));
});

// Identity konfigürasyonu
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<OdevDagitimDbContext>();

>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
// Notyf konfigürasyonu
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

<<<<<<< HEAD
// SignalR servisini ekle
builder.Services.AddSignalR();

// CORS ayarlarını ekle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000") // Güvenilir istemci adresleri
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // SignalR için gerekli
    });
});

// Razor Pages ve MVC için gerekli servisleri ekle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

=======
>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
var app = builder.Build();

// Middleware sıralaması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

<<<<<<< HEAD
app.UseAuthentication(); // Authentication middleware
app.UseAuthorization();

app.UseCors(); // CORS middleware

// SignalR hub rotasını ekle
app.MapHub<GeneralHub>("/Hubs/generalhub");

=======
app.UseAuthentication(); // Authentication'ı ekleyin
app.UseAuthorization();

>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
// Route yapılandırması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

