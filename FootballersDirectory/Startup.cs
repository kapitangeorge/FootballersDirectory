using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballersDirectory.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballersDirectory
{
    public class Startup
    {
        private IConfigurationRoot confRoot;
        
        public Startup(IWebHostEnvironment webHost)
        {
            confRoot = new ConfigurationBuilder().SetBasePath(webHost.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<AppDBContext>(options => options.UseNpgsql(confRoot.GetConnectionString("DefaultConnection")));
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Footballer}/{action=AllFootballers}");
            });
        }
    }
}
