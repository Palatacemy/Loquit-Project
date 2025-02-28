using Loquit.Data;
using Loquit.Data.Repositories.Abstractions;
using Loquit.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Loquit.Services.Services.ChatTypesServices;
using Loquit.Services.Services.MessageTypesServices;
using Loquit.Services.Services;
using Loquit.Data.Entities;
using Microsoft.Extensions.Options;
using Loquit.Data.Seeders;
using Loquit.Services.Services.Abstractions;
using Loquit.Services.Services.Abstractions.ChatTypesAbstractions;
using Loquit.Services.Services.Abstractions.MessageTypesAbstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddTransient<IDirectChatRepository, DirectChatRepository>();
builder.Services.AddTransient<IGroupChatRepository, GroupChatRepository>();
builder.Services.AddTransient<IChatUserRepository, ChatUserRepository>();
builder.Services.AddTransient<IDirectChatService, DirectChatService>();
builder.Services.AddTransient<IGroupChatService, GroupChatService>();
builder.Services.AddTransient<IImageMessageService, ImageMessageService>();
builder.Services.AddTransient<ITextMessageService, TextMessageService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    RoleSeeder.Initialize(services).Wait();
    AdminSeeder.Initialize(services).Wait();
    // TestProfilesSeeder.Initialize(services, 5).Wait();
    AlgorithmTestSeeder.Initialize(services, false).Wait();
}
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