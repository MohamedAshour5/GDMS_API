using GDMS_API.Interfaces;
using GDMS_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GDMS_API.HelperFunction;
using GDMS_Contracts.Repository;
using GDMS_Contracts.IRepository;
using GDMS_DAL.Context;
using Microsoft.EntityFrameworkCore;
using GDMS_DAL.ComonManager;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var MyAllowSpecificOrigins = "RM_Cors";
var ListCros = configuration.GetSection("CorsOrigins");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(ListCros.Get<List<string>>().ToArray())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ApiService>();
builder.Services.AddTransient<IService, ApiService>();
builder.Services.AddHttpClient<RegistrationRepo>();
builder.Services.AddTransient<IRegistrationRepo, RegistrationRepo>();
builder.Services.AddHttpClient<CommonHelperFunction>();
builder.Services.AddTransient<ICommonHelperFunction, CommonHelperFunction>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "admin",
        ValidAudience = "admin",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings").GetSection("SecretKey").GetSection("Digital").Value))
    };
});
//builder.Services.AddDbContext<RM_DbContext>(e => e.UseSqlServer(EncryptionManager.DecryptString(configuration.GetSection("GDMS_DBEntities").Value)));
builder.Services.AddDbContext<GDMS_DbContext>(e => e.UseSqlServer(configuration.GetSection("GDMS_DBEntities").Value));
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    context.Response.Headers.Append("Cross-Origin-Resource-Policy", "same-origin");
    await next();
});
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
