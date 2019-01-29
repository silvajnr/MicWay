using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicwayTech.DAL;
using MicwayTech.Data;
using Swashbuckle.AspNetCore.Swagger;
using System.Threading.Tasks;

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

            //Enabling Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
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

            app.UseSwagger(c => { c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value); });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.Run(async (context) => await Task.Run(() => context.Response.Redirect("/swagger")));

        }
    }
}
