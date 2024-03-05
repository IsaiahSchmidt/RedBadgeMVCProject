using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using musicProject.Data;
using musicProject.Data.Data;
using musicProject.Data.Entities;
using musicProject.Services.AlbumReviewServices;
using musicProject.Services.AlbumServices;
using musicProject.Services.ArtistServices;
using musicProject.Services.TrackReviewServices;
using musicProject.Services.TrackServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IAlbumReviewService, AlbumReviewService>();
builder.Services.AddScoped<ITrackReviewService, TrackReviewService>();

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
