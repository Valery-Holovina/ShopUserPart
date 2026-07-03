
//dotnet new web -n MyWebApp && cd MyWebApp && dotnet run
//dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL && dotnet add package Microsoft.EntityFrameworkCore.Tools
// dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
// dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
// dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0


//dotnet clean
//dotnet build

//dotnet ef migrations add InitialCreate
//dotnet ef database update

// dotnet ef migrations add SecondCreate
// dotnet ef database update



//dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
// dotnet add package Microsoft.EntityFrameworkCore
// dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
//dotnet add package Microsoft.AspNetCore.Identity.UI

using Microsoft.EntityFrameworkCore;
using Shop_P41.Services;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// =========================
// DATABASE (PostgreSQL)
// =========================
builder.Services.AddDbContext<ShopContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =========================
// IDENTITY (WITH ROLES)
// =========================
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;

    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ShopContext>()
.AddDefaultTokenProviders();

// =========================
// SERVICES
// =========================
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// =========================
// MIDDLEWARE
// =========================
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =========================
// ROUTES
// =========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"
);

app.Run();