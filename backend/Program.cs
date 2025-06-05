using backend.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fraud Detection API",
        Version = "v1"
    });
});

// Configure Entity Framework Core with PostgreSQL
//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("connection string to db")));

//in memory db for testing purposes
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));

builder.Services.AddIdentity<AppUser, AppUserRoles>(options =>
{
    //options.Password.RequireDigit = true;
    //options.Password.RequiredLength = 6;
    //options.Password.RequireLowercase = true;
    //options.Password.RequireUppercase = true;
    //options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<AppUserRoles>>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fraud Detection API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var createAdminRole = async (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppUserRoles>>();
    
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var role = new AppUserRoles { Name = "Admin" };
        await roleManager.CreateAsync(role);
    }
};

app.Run();
