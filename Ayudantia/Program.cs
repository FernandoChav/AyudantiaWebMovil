
using System.Security.Claims;
using System.Text;

using API.middleware;

using Ayudantia.Src.Data;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Models;
using Ayudantia.Src.Repositories;
using Ayudantia.Src.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;
Log.Logger = new LoggerConfiguration()
    .CreateLogger();
try
{
    Log.Information("starting server.");

    // creacion de patron del builder de .net para crear la aplicacion
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddTransient<ExceptionMIddleware>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    builder.Services.AddScoped<ITokenServices, TokenService>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<StoreContext>();
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SignInKey"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });
    builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName());

    // crearmos la aplicacion con todo lo que se agrega al patron de diseño builder
    // y se le asigna el nombre de app
    var app = builder.Build();
    app.UseMiddleware<ExceptionMIddleware>();
    await DbInitializer.InitDb(app);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    // corremos la aplicacion
    // app.Run() es el metodo que se encarga de correr la aplicacion y escuchar las peticiones
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}