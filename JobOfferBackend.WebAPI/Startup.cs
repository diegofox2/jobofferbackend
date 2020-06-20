using JobOfferBackend.ApplicationServices;
using JobOfferBackend.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace JobOfferBackend.WebAPI
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
            var mongo = new MongoClient();

            services.AddSingleton(mongo.GetDatabase("JobOfferDatabase"));
            services.AddScoped<CompanyRepository>();
            services.AddScoped<JobOfferRepository>();
            services.AddScoped<RecruiterRepository>();
            services.AddScoped<AccountRepository>();
            services.AddScoped<RecruiterService>();
            services.AddScoped<JobOfferService>();
            services.AddScoped<AccountService>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Offer API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseMvc();
            app.UseSwagger();

            

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Offer API v1");
            });
        }
    }
}
