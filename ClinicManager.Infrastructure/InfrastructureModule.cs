﻿using ClinicManager.Core.IRepositories;
using ClinicManager.Infrastructure.Persistence;
using ClinicManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
namespace ClinicManager.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddPersistence(configuration)
                .AddRepositories()
                .AddUnitOfWork();
                //.AddAuthentication(configuration);

            return services;
        }
        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ClinicManagerDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>();
            services.AddScoped<IServiceNoteRepository, ServiceNoteRepository>();

            return services;
        }

        //private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services
        //        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidateAudience = true,
        //                ValidateLifetime = true,
        //                ValidateIssuerSigningKey = true,

        //                ValidIssuer = configuration["Jwt:Issuer"],
        //                ValidAudience = configuration["Jwt:Audience"],
        //                IssuerSigningKey = new SymmetricSecurityKey
        //                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        //            };
        //        });

        //    services.AddScoped<IAuthService, AuthService>();

        //    return services;
        //} 

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }

}
