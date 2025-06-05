using Microsoft.AspNetCore.Authentication.Cookies;
using WebAPI.Client.Helpers;
using WebAPI.Client.Options;
using WebAPI.Client.Repository;
using WebAPI.Client.Repository.Account;
using WebAPI.Client.Repository.ApplicationUserInfo;
using WebAPI.Client.Repository.MoodType;
using WebAPI.Client.Repository.Post;
using WebAPI.Client.Repository.PostComment;
using WebAPI.Client.Repository.PostType;
using WebApp.Helpers;
using WebApp.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();



builder.Services.Configure<AppOptions>(configuration.GetSection("AppOptions"));

builder.Services.Configure<HttpClientOptions>(configuration.GetSection("HttpClientOptions"));


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMoodTypeRepository, MoodTypeRepository>();
builder.Services.AddScoped<IApplicationUserInfoRepository, ApplicationUserInfoRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostCommentRepository, PostCommentRepository>();
builder.Services.AddScoped<IPostTypeRepository, PostTypeRepository>();


builder.Services.AddScoped<IJwtTokenAuthenticationHandler, JwtTokenAuthenticationHandler>(); // Handles storing and retrieving JWT tokens


builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient(configuration["HttpClientOptions:ClientName"], client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientOptions:BaseAddress"]);
}
);


// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to the login page
        options.LogoutPath = "/Account/Logout"; // Path to the logout page
        options.AccessDeniedPath = "/Account/Login"; // Path to the access denied page
        options.ExpireTimeSpan = TimeSpan.FromDays(1); // Cookie expiration time
        options.SlidingExpiration = true; // Enable sliding expiration
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



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
