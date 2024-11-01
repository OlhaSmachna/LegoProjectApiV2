using LegoProjectApiV2.DAL.Categories;
using LegoProjectApiV2.DAL.Colors;
using LegoProjectApiV2.DAL.Lists;
using LegoProjectApiV2.DAL.Materials;
using LegoProjectApiV2.DAL.Parts;
using LegoProjectApiV2.DAL.Users;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Services.Bricks;
using LegoProjectApiV2.Services.Categories;
using LegoProjectApiV2.Services.Colors;
using LegoProjectApiV2.Services.Lists;
using LegoProjectApiV2.Services.Materials;
using LegoProjectApiV2.Services.Users;
using LegoProjectApiV2.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DockerDbConn");
builder.Services.AddDbContext<LegoProjectDB>(options => options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("Secret").Value))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "LegoProject API",
        Description = "Here you can see our api routes.",
    });
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<JwtHandler>();
builder.Services.AddTransient<Users_IDAL, Users_DAL>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<Categories_IDAL, Categories_DAL>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<Parts_IDAL, Parts_DAL>();
builder.Services.AddTransient<IBrickService, BrickService>();
builder.Services.AddTransient<Lists_IDAL, Lists_DAL>();
builder.Services.AddTransient<IListService, ListService>();
builder.Services.AddTransient<Materials_IDAL, Materials_DAL>();
builder.Services.AddTransient<IMaterialService, MaterialService>();
builder.Services.AddTransient<Colors_IDAL, Colors_DAL>();
builder.Services.AddTransient<IColorService, ColorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "lego_project_api/swagger/{documentname}/swagger.json";
    });

    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/lego_project_api/swagger/v2/swagger.json", "LegoProject API V2");
        c.RoutePrefix = "lego_project_api/swagger";
    });
}

app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
