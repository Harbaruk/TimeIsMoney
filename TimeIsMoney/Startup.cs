using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using TimeIsMoney.Api.Attributes;
using TimeIsMoney.Api.Providers;
using TimeIsMoney.Common;
using TimeIsMoney.Common.EmailSender;
using TimeIsMoney.CompositionRoot;
using TimeIsMoney.Crypto;
using TimeIsMoney.DataAccess;
using TimeIsMoney.Extensions;
using TimeIsMoney.Services.Crypto;
using TimeIsMoney.Services.ProviderAbstraction;
using TimeIsMoney.Services.Token;

namespace TimeIsMoney
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    AllowIntegerValues = false,
                    CamelCaseText = false
                });
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            })
          .AddFluentValidation(o =>
          {
              o.RegisterValidatorsFromAssemblyContaining<Startup>();
          });

            services.AddCors(o => o.AddPolicy("CORS", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            Bootstrap.RegisterServices(services);
            services.AddScoped(typeof(DomainTaskStatus));
            services.AddScoped(typeof(ValidateModelAttribute));
            services.AddScoped<IAuthenticaterUserProvider, AuthenticatedUserProvider>();
            services.AddSingleton<ICryptoContext, AspNetCryptoContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<TimeInMoneyContext>(o =>
            {
                string connStr = Configuration.GetConnectionString(_hostingEnvironment.EnvironmentName);
                if (String.IsNullOrWhiteSpace(connStr))
                {
                    throw new Exception($"No connection string defined for {_hostingEnvironment.EnvironmentName}");
                }
                o.UseSqlServer(connStr);
            }, ServiceLifetime.Scoped);

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("TIM", new Info
                {
                    License = new License { Name = "Free" },
                    Version = "v1"
                }));

            services.ConfigureFromSection<CryptoOptions>(Configuration);
            services.ConfigureFromSection<JwtOptions>(Configuration);
            services.ConfigureFromSection<RedirectOptions>(Configuration);
            services.ConfigureFromSection<EmailOptions>(Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ClockSkew = TimeSpan.Zero,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = Configuration.GetSection("Jwt")["ValidIssuer"],
                 ValidAudience = Configuration.GetSection("Jwt")["ValidAudience"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt")["Key"]))
             };
         });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CORS");

            app.UseAuthentication();

            app.UseSwagger();

            app.UseStaticFiles();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PTS V1");
            });

            app.UseMvc();
        }
    }
}