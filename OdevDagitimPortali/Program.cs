using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
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

// Repository'leri Scoped olarak kaydet
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SubmissionRepository>();
builder.Services.AddScoped<AssignmentRepository>();

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

// Notyf konfigürasyonu
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

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

app.UseAuthentication(); // Authentication'ı ekleyin
app.UseAuthorization();

// Route yapılandırması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

