using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<LinkService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
builder.Services.AddHttpClient<LoginService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
builder.Services.AddHttpClient<FileService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});
builder.Services.AddHttpClient<UserService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);

}); builder.Services.AddHttpClient<QuotaService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "MyWebsiteCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.LoginPath = "/Login/Index";
    options.LogoutPath = "/Login/Logout";
    options.AccessDeniedPath = "/Login/Index";
    options.SlidingExpiration = true;
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

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
