using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicwayTech.DAL;
using MicwayTech.Data;

namespace MicwayTech.API
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
            //Injecting IdriverDataStore for IOC
            services.AddTransient<IDriverDataStore, DriverDataStore>();

            //Normally Connection strings are inside appsettings or vault because it's a local file DB i decided to insert here
            var connection = @"Server=(localdb)\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=Micway.NewDb;Trusted_Connection=True;ConnectRetryCount=0;MultipleActiveResultSets=true";
            //EF Core initialization
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(connection));

            ////Removes Model States Validator
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
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
