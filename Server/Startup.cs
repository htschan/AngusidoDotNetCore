using System.Security.Claims;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Auth;
using Server.Controllers;
using Server.Data;
using Server.Domain.UserConfiguration;
using Server.Domain.UserConfiguration.Repositories;
using Server.Helpers;
using Swashbuckle.AspNetCore.Swagger;

namespace Server
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"server.appsettings.server.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsDevPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            services.AddOptions();
            // Register the IConfiguration instance which TpConfigOptions binds against.
            services.Configure<TpMailConfigOptions>(Configuration.GetSection("AnMailConfigOptions"));

            // Add framework services.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();
            services.AddTransient<DbInitializer>();

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddSingleton<IHolidayService, HolidayService>();
            services.AddSingleton<ITimeService, TimeService>();
            services.AddTransient<IAnController, AnControllerImpl>();
            services.AddTransient<AppUser>();
            services.AddTransient<IdentityRole>();
            services.AddTransient<AppUserManager>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireApiUserRole", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Strings.JwtClaims.ApiAccess));
                options.AddPolicy("RequireApiAdminRole", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Strings.JwtClaims.ApiAccessAdmin));
            });

            services.AddIdentity<AppUser, IdentityRole>
                (o =>
                {
                    // configure identity options
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                    o.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper();

            ConfigureJwtAuthService(services);

            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Angusido", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbInitializer dbInitializer)
        {
            app.UseCors("CorsDevPolicy");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/AnDotNet-{Date}.log");

            app.UseRewriter(new RewriteOptions().Add(new Rewriter
            {
                ExcludeLocalhost = false
            }));

            dbInitializer.Initialize();

            app.UseAuthentication();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Angusido API V1");
            });
        }
    }
}
