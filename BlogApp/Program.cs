using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options=>{
    var config = builder.Configuration;
    var connString = config.GetConnectionString("database");
    options.UseSqlite(connString);
});
builder.Services.AddDbContext<IdentityContext>(options=>{
    var config = builder.Configuration;
    var connectionString=config.GetConnectionString("database");
    options.UseSqlite(connectionString);
});
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<IdentityContext>();


builder.Services.AddScoped<IPostRepository,EfPostRepository>();
builder.Services.AddScoped<ICommentRepository,EfCommentRepository>();
builder.Services.AddScoped<IGroupRepository,EFGroupRepository>();
builder.Services.AddScoped<IUserRepository,EfUserRepository>();

builder.Services.AddScoped<IMembershipRequestRepository,EfMembershipRequestRepository>();
builder.Services.AddScoped<IGroupService,EfGroupService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>
{options.LoginPath="/Users/Login";
});//cookie kullanacağımızı uygulamaya söyledik

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
