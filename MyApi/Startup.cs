using iel.imot.Helper;
using JN.Authentication.Interfaces;
using JN.Authentication.Scheme;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApi.Core.Authorization;
using MyApi.Core.Helpers;
using MyApi.Core.Middleware;
using MyApi.Core.Services.Users;
using MyApi.Data.Models.Context;
using MyApi.Filters;
using MyApi.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyApi
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

            services.AddCors();
            // configure strongly typed settings objects



            // add session 
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // add session
            var secret = $"{Configuration["AppSettings:Secret"]}"; // Configuration.GetSection("AppSettings:Secret");

            var key = Encoding.ASCII.GetBytes(secret);

            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton(Configuration);
            services.AddHttpClient();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection"),
              sqlServerOptionsAction: sqlOptions =>
               {
                   sqlOptions.EnableRetryOnFailure(
                       maxRetryCount: 10,
                       maxRetryDelay: TimeSpan.FromSeconds(30),
                       errorNumbersToAdd: null
                       );
               }
            )
            );

            services.AddControllers().AddJsonOptions(s =>
            {
                s.JsonSerializerOptions.PropertyNamingPolicy = null;
            });


            services.AddMvc(options =>
            {

            }).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                o.SerializerSettings.Formatting = Formatting.None;
                o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                o.SerializerSettings.Converters.Add(new TrimmingConverter());
                o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });


            services.AddDataProtection();

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string> ()
                    }
                });


                c.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme()
                {
                    Description = "Authorization by x-api-key inside request's header",
                    Name = "X-API-KEY",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                   { key, new List<string>() }
                };

                c.AddSecurityRequirement(requirement);
                //c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                //{
                //    Name = "x-api-key",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Description = "Authorization by x-api-key inside request's header",
                //    Scheme = "ApiKeyScheme"
                //});

               
                //var requirement = new OpenApiSecurityRequirement
                //{
                //   { key, new List<string>() }
                //};

                //c.AddSecurityRequirement(requirement);

                c.OperationFilter<AuthOperationFiler>();
            });


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });



            //services.AddAuthorization(options => { 



            //});

            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApi v1");
                    //Collapse model near example.
                    c.DefaultModelExpandDepth(0);
                    //Remove separate model definition.
                    c.DefaultModelsExpandDepth(-1);
                });

            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
           //.AllowCredentials()
           );

            app.UseAuthentication();
            app.UseAuthorization();
            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirect("^$", "/swagger");
            app.UseRewriter(rewriteOptions);
        }
    }
}
