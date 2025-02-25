using Assignment1MVC.Repositories;
using Assignment1MVC.Services;
using Business.Interfaces;
using Business.Services;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
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



builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession((option) =>
{
    option.Cookie.Name = "TienThuatCooking";
    option.IdleTimeout = new TimeSpan(0, 30, 0);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/", async context =>
//    {
//        int? count;
//        count = context.Session.GetInt32("count"); //doc session
//        await context.Response.WriteAsync($"Hello world{count}");


//    });

//    endpoints.MapGet("/readwritesession", async (context) =>
//    {
//        int? count;

//        count = context.Session.GetInt32("count"); //doc session
//        if (count == null)
//        {
//            count = 0;
//        }
//        count += 1;
//        context.Session.SetInt32("count", count.Value); //luu session

//        await context.Response.WriteAsync($"So lan truy cap la: {count}");
//    });
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
