using BlogApp.Data;
using BlogApp.Repositories.Abstract;
using BlogApp.Repositories.Concreate.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
 builder.Services.AddDbContext<BlogContext>(options=>{
      var config = builder.Configuration;
     var connectionString=config.GetConnectionString("mysql");
     var version= new MySqlServerVersion(new Version(8,2,0));
     options.UseMySql(connectionString,version);
 });
 builder.Services.AddDbContext<IdentityContext>(options=>{
     var config = builder.Configuration;
     var connectionString=config.GetConnectionString("mysql");
     var version= new MySqlServerVersion(new Version(8,2,0));
     options.UseMySql(connectionString,version);
  });

builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options=>{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;

});

builder.Services.AddScoped<IPostRepository,EfPostRepository>();
builder.Services.AddScoped<ICommentRepository,EfCommentRepository>();
builder.Services.AddScoped<IGroupRepository,EFGroupRepository>();
builder.Services.AddScoped<IUserRepository,EfUserRepository>();
builder.Services.AddScoped<IMembershipRequestRepository,EfMembershipRequestRepository>();
builder.Services.AddScoped<IGroupService,EfGroupService>();
builder.Services.AddScoped<IEmailSender,SendEmail>(i=>new SendEmail(
    builder.Configuration["EmailSender:Host"],
    builder.Configuration.GetValue<int>("EmailSender:Port"),
    builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
    builder.Configuration["EmailSender:username"],
    builder.Configuration["EmailSender:password"]

));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>
{options.LoginPath="/Users/Login";
});//cookie kullanacağımızı uygulamaya söyledik
builder.Services.ConfigureApplicationCookie(options=>{
    options.LoginPath="/Users/Login";
});
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
