using JobOfferBackend.ApplicationServices;
using JobOfferBackend.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

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
            services.AddScoped<PersonRepository>();
            services.AddScoped<SkillRepository>();
            services.AddScoped<RecruiterService>();
            services.AddScoped<JobOfferService>();
            services.AddScoped<PersonService>();
            services.AddScoped<AccountService>();
            services.AddScoped<SkillsService>();
            services.AddScoped<CompaniesService>();
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSession();

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

            app.UseAuthentication();

            app.UseSession();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Offer API v1");
            });
        }
    }
}
