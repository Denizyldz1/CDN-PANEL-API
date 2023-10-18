using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Entities;
using AppouseProject.Data;
using AppouseProject.Data.Repositories;
using AppouseProject.Data.UnitOfWorks;
using AppouseProject.Service.Mapping;
using AppouseProject.Service.Services;
using AppouseProjet.API.Filters;
using AppouseProjet.API.ImageService;
using AppouseProjet.API.MailServices;
using AppouseProjet.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<CustomTokenOption>(opt => builder.Configuration.GetSection("TokenOption").Bind(opt));
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();

builder.Services.Configure<ApiBehaviorOptions>(x =>
{
    x.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped<IMailService, MailService>();


builder.Services.AddScoped(typeof(AuthorityControl));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IFileRepository,FileRepository>();

builder.Services.AddScoped<IQuotaRepository,QuotaRepository>();
builder.Services.AddScoped<IQuotaService,QuotaService>();

builder.Services.AddScoped<InterfaceImageService,ImageService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), opt =>
    {
        opt.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.User.RequireUniqueEmail = true;
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.AllowedForNewUsers = false;
    opt.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

//Kimlik doðrulama mekanizmasý
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        //Gelen token kontrol edilmesi
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCustomException();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
