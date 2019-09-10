using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CE.API.Services;
using AutoMapper;
using CE.API.Services.PaginationServices;
using CE.API.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace CE.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
            .AddXmlDataContractSerializerFormatters();
            // Using connections strings
            var connectionStrings = Configuration["ConnectionStrings:CEDBConnectionString"];
            // Registering dbcontext in the container
            services.AddDbContext<Entities.CEDatabaseContext>(o => o.UseSqlServer(connectionStrings));

            // Adding AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles.UsuarioProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Registering Repositories
            services.AddScoped<IUserRolesRepository, UserRoleRepository>();

            // Registering Filters
            services.AddScoped<Filters.UserMinInfoResultFilterAttribute>();
            services.AddScoped<Filters.UsersMinInfoResultFilterAttribute>();

            // Registering Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Transient is used for lightweight services
            // services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient(typeof(IPropertyMappingService<,>), typeof(PropertyMappingService<,>));
            services.AddTransient<IApplySort, ApplySort>();

            services.AddTransient<ICreateResourceUri, CreateResourceUri>();

            // This service uses Microsoft.AspNetCore.Infrastructure
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // this service uses IActionContextAccessor
            // and, Microsoft.AspNetCore.Mvc.Routing.UrlHelper
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext =
                implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            // Services for cache headers
            services.AddHttpCacheHeaders((expirationOptions) => {
                expirationOptions.MaxAge = 600;
            }, (validationModelOptions) => {
                validationModelOptions.MustRevalidate = true;
            });

            // Adding store caching services
            services.AddResponseCaching();

            // Adding limit services
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>((options) => {
                options.GeneralRules =
                new List<RateLimitRule>() {
                    // Accept only 10 request in 5 minutes
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 1000,
                        Period = "5m"
                    },
                    // and, two request every 10 seconds
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 200,
                        Period = "10s"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            // Adding versioning support
            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("X-api-version"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Use limit services before any middleware
            app.UseIpRateLimiting();

            // Adding response caching before cache middleware
            app.UseResponseCaching();

            // Adding cache middleware important add before UseMvc
            app.UseHttpCacheHeaders();

            app.UseMvc();
        }
    }
}
