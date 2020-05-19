using AutoMapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using SmartLibrary.BLL.MapperProfiles;
using SmartLibrary.BLL.Services.Abstraction;
using SmartLibrary.BLL.Services.Implementation;
using SmartLibrary.DAL;
using SmartLibrary.DAL.Entities;
using SmartLibrary.DAL.Repostiry.Abstraction;
using SmartLibrary.DAL.Repostiry.Implementation;
using SmartLibrary.API.Middleware;
using SmartLibrary.API.Hubs;
using FluentValidation.AspNetCore;
using SmartLibrary.DTO.Models.Account;
using FluentValidation;
using SmartLibrary.API.ValidatorModels;
using SmartLibrary.DTO.Models.AvailableBook;
using SmartLibrary.DTO.Models.Book;

namespace SmartLibrary.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(cfg =>
                    {
                        cfg.RequireHttpsMetadata = false;
                        cfg.SaveToken = true;
                        cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = Configuration["JwtIssuer"],
                            ValidAudience = Configuration["JwtAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                            ClockSkew = TimeSpan.Zero
                        };
                        cfg.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                context.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }
                        };
                    });

            // DbContext Configuration
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartLibrary.API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer token'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    }
                );
            });

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(
                        "http://localhost:3000",
                        "https://localhost:3000",
                        "https://smartlibraryweb.azurewebsites.net"
                );
            }));

            //Mapper Configuration
            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountProfile());
                cfg.AddProfile(new BookProfile());
                cfg.AddProfile(new AvailableBookProfile());
                cfg.AddProfile(new ReservedBookProfile());
            }).CreateMapper());

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAvailableBookService, AvailableBookService>();
            services.AddTransient<IReservedBookService, ReservedBookService>();

            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<AccountRegistrationDto>, AccountRegistrationValidator>();
            services.AddTransient<IValidator<AccountLoginDto>, AccountLoginValidator>();
            services.AddTransient<IValidator<AccountAuthorizeDto>, AccountAuthorizeValidator>();
            services.AddTransient<IValidator<AccountUpdateDto>, AccountUpdateValidator>();
            services.AddTransient<IValidator<AvailableBookNewDto>, AvailableBookNewValidator>();
            services.AddTransient<IValidator<BookNewDto>, BookNewValidator>();
            services.AddTransient<IValidator<BookUpdateDto>, BookUpdateValidator>();
            //services.AddTransient<IValidator<BookUpdateDto>, BookUpdateValidator>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartLibrary.API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddleware<ErrorMiddleware>();

            app.UseSignalR(routes => routes.MapHub<ChatHub>("/chathub"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
