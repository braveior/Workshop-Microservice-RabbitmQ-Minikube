using Braveior.BuddyRewards.API.Extensions;
using Braveior.BuddyRewards.Service;
using Braveior.BuddyRewards.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Braveior.BuddyRewards.API
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
            services.AddCors(options =>
            {


                options.AddPolicy("CorsPolicy",
                  builder =>
                  {
                      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                  });
            });
            services.AddControllers();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IDataService, DataService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Buddy Rewards API", Version = "v1.0" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.ConfigureExceptionHandler(Log.Logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();

           
            app.UseSwaggerUI(ui =>
            {
                ui.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Buddy Rewards API Endpoint");
            });
        }
    }
}
