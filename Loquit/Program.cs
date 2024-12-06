using Loquit.Data;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Loquit.Services.Abstractions.ChatTypesAbstractions;
using Loquit.Services.Services.ChatTypesServices;
using Loquit.Services.Abstractions.MessageTypesAbstractions;
using Loquit.Services.Services.MessageTypesServices;
using Loquit.Services.Abstractions;
using Loquit.Services.Services;
using Loquit.Data.Entities;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient(typeof(ICrudRepository<>), typeof(CrudRepository<>));
builder.Services.AddTransient<IDirectChatService, DirectChatService>();
builder.Services.AddTransient<IGroupChatService, GroupChatService>();
builder.Services.AddTransient<IImageMessageService, ImageMessageService>();
builder.Services.AddTransient<ITextMessageService, TextMessageService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IPostService, PostService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();

//comment lololol