using Assignment1MVC.Repositories;
using Assignment1MVC.Services;
using Assignment2.Utils;
using Business.Interfaces;
using Business.Services;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<FunewsManagementContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FuNewDB")));
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddSignalR();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Auth/Login";
		options.LogoutPath = "/Auth/Logout";
		options.AccessDeniedPath = "/AccessDenied";
	});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffOnly", policy =>
        policy.RequireClaim("AccountRole", "1"));
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<FunewsManagementContext>();
	var adminEmail = builder.Configuration["AdminAccount:Email"];
	var adminPassword = builder.Configuration["AdminAccount:Password"];
	var adminRoleId = builder.Configuration.GetSection("AdminRole:RoleId").Get<int>();

	if (!dbContext.SystemAccounts.Any(a => a.AccountEmail == adminEmail))
	{
		dbContext.SystemAccounts.Add(new SystemAccount
		{
			AccountId = 1,
			AccountEmail = adminEmail,
			AccountPassword = adminPassword, // Nên mã hóa trong thực tế
			AccountName = "Admin",
			AccountRole = adminRoleId,
			Status = 1
		});
		dbContext.SaveChanges();
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapHub<CommentHub>("/commentHub");

app.Run();
