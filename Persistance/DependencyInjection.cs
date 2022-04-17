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

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration ) 
        {

            services.Configure<Jwt>(cfg => configuration.GetSection("JWT"));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options=>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );


            return services;
        }
    }
}
