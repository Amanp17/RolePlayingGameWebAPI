using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RolePlayingGameWebAPI.Data;
using RolePlayingGameWebAPI.Services;
using RolePlayingGameWebAPI.Services.Fight;
using RolePlayingGameWebAPI.Services.Weapon;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI
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

            services.AddControllers();

            //AddSecurityDefinition Helps to Add one or more "securityDefinitions", describing how your API is protected, to the generated Swagger
            //OpertaionFilter Extend the Swagger Generator with "filters" that can modify Operations after they're initially generated
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RolePlayingGameWebAPI", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer Scheme. Example : \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            //Registers services required by authentication services. defaultScheme specifies the name of the scheme to use
            //JWT bearer authentication performs authentication by extracting and validating a JWT token from the Authorization request header.
            //TokenValidationParameters Contains types and defination to Validate Token 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=> {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration.GetSection("JwtToken:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IWeaponService, WeaponService>(); 
            services.AddScoped<IFightService, FightService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RolePlayingGameWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
