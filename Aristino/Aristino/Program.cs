using Aristino.Mapping;
using Aristino.Models;
using Aristino.Repository;
using Aristino.SendMail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AristinoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Aristino")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService,MailService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(p =>
{
	p.Cookie.Name = "UserLogin";	
	p.LoginPath = "/Login/Index";
	p.ExpireTimeSpan = TimeSpan.FromDays(1);
	p.AccessDeniedPath = "/Forbidden/403";
})
	.AddCookie("AdminLogin",options =>
	{
		options.Cookie.Name = "AdminLogin";
		options.LoginPath = "/AdminLogin/Login";
		options.AccessDeniedPath = "/Forbidden/403";
	});
builder.Services.AddAuthorization(policy =>
{
	policy.AddPolicy("ForStaff", option => option.RequireAssertion(context => context.User.HasClaim(claim => claim.Value == "Staff" || claim.Value == "Admins")));
	policy.AddPolicy("AdminOnly", option => option.RequireAssertion(context => context.User.HasClaim(claim => claim.Value == "Admins")));
	policy.AddPolicy("NotLockedAccount", option => option.RequireAssertion(context => context.User.HasClaim(claim => claim.Value == "1")));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(session =>
{
	session.Cookie.Name = "UserSession";
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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
