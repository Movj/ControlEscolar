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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Using connections strings
            var connectionStrings = Configuration["ConnectionStrings:CEDBConnectionString"];
            // Registering dbcontext in the container
            services.AddDbContext<Entities.CEDatabaseContext>(o => o.UseSqlServer(connectionStrings));

            // Adding AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new AutoMapperProfiles.UsuarioProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Registering Repositories
            services.AddScoped<IUserRolesRepository, UserRoleRepository>();

            // Registering Filters
            services.AddScoped<Filters.UsuarioMinInfoResultFilterAttribute>();
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
            app.UseMvc();
        }
    }
}
