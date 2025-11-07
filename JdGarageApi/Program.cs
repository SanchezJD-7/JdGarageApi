using System.Text;
using JdGarageApi.BikesMappers;
using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository;
using JdGarageApi.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using XAct;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "JD Garage Api",
        Description = "Garage Api",
        //TermsOfService = new Uri("https://render2web.com/promociones"),
        //Contact = new OpenApiContact
        //{
        //Name = "render2web",
        //Url = new Uri("https://render2web.com/promociones")
        //},
    });
 
});

//Repositorios
builder.Services.AddScoped<IBikeCategoryRepository, BikeCategoryRepository>();
builder.Services.AddScoped<IBikeRepository, BikeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandsRepository>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

//Soporte para autenticación con .NET Identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
    //option.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version")//?api-version=1.0
    //);
});

apiVersioningBuilder.AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
 );

//Automapper
builder.Services.AddAutoMapper(typeof(BikesMapper));

//Autenticación
builder.Services.AddAuthentication(
    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
 });

//Soporte para CORS
//Se puede habilitar: 1/.Un dominio - 2/.Múltiples dominios - 3/.Cualquier dominio (Tener en cuenta la seguridad para este caso)
//Usamos de ejemplo el dominio: http://localhost:3001, se debe cambiar por el correcto
//Se usa (*) para habilitar todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "JdGarageApiV1");
    });
}

//Soporte para archivos estáticos como imágen
app.UseStaticFiles();   

app.UseHttpsRedirection();

//Soporte para Cors
app.UseCors("PolicyCors");

//Soporte para autenticación
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
