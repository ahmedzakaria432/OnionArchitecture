using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;
using Infrastructure.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Peresistence.Data;

using Microsoft.EntityFrameworkCore;
using Application.Samples;
using Application.Samples.Interfaces;
using Core.Samples;
using Infrastructure.Peresistence.Samples;
using Core.Shared;
using Infrastructure.Peresistence.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Application.Identity;
using Infrastructure.Infrastructure.Helpers.Middlewares;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<Jwt>(cfg => configuration.GetSection("JWT"));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped<IIdentityService,IdentityService>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<ISampleService, SampleService>();



            return services;
        }
    }
}
