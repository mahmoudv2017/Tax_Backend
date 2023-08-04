using Core.Entities;
using Core.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddDbContext<AppIdentityContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("IdentityDB"));
});

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
   
});

identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);

identityBuilder.AddEntityFrameworkStores<AppIdentityContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();
identityBuilder.AddRoleManager<RoleManager<IdentityRole>> ();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token:Key").Value));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsDefault", opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});


builder.Services.AddAutoMapper(typeof( Api.Helpers.MappingProfiles).Assembly);

builder.Services.AddScoped<ITokenProvider, TokenService>();
builder.Services.AddScoped( typeof(IUnitOfWork<>) , typeof(UnitOfWork<>));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



var scope = app.Services.CreateScope();

var IdentityContext = scope.ServiceProvider.GetRequiredService<AppIdentityContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
var Usermanager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

try
{
    await IdentityContext.Database.MigrateAsync();

    await AppIdentityContextSeed.SeedAsync(Usermanager, rolemanager , IdentityContext);

}
catch (Exception ex)
{
    logger.LogError(ex, ex.Message);
}

app.UseCors("CorsDefault");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.MapControllers();

app.Run();
