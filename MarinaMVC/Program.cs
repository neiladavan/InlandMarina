using MarinaData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme). //added
            AddCookie(opt => opt.LoginPath = "/Customer/Login"); // what is the login page

// Add DB Context
builder.Services.AddDbContext<InlandMarinaContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("InlandMarinaContext")));

builder.Services.AddSession(); // to use Session state object

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection(); // added
app.UseStatusCodePages(); // added: for more user friendly error pages for 403 & 404
app.UseStaticFiles();

app.UseSession(); // should be define before routes
app.UseRouting();

app.UseAuthentication(); // added
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
