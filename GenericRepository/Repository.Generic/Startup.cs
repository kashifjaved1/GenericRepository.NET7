﻿using Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.Generic
{
    /// <summary>
    /// This class includes Service Collection extensions
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// This method takes <see cref="ServiceLifetime"/> service lifetime and <see cref="{TDbContext}"/> database context. In additional this method performs apply easy repository library for own db context
        /// </summary>
        /// <typeparam name="TDbContext">
        /// <see cref="DbContext"/> database context.
        /// </typeparam>
        /// <param name="services">
        /// Service collection <see cref="IServiceCollection"/>
        /// </param>
        /// <param name="serviceLifetime">
        /// Service LifeTime <see cref="ServiceLifetime"/>
        /// </param>
        /// <returns>
        /// <see cref="IServiceCollection"/>
        /// </returns>
        public static IServiceCollection ApplyGenericRepository<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where TDbContext : DbContext
        {
            services.Add(new ServiceDescriptor(
                typeof(IRepository),
                serviceProvider =>
                {
                    var dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    return new Repository(dbContext);
                },
                serviceLifetime));

            services.Add(new ServiceDescriptor(
                typeof(IUnitOfWork),
                serviceProvider =>
                {
                    var repository = serviceProvider.GetService<IRepository>();
                    return new UnitOfWork(repository);
                },
                serviceLifetime));
            return services;
        }
    }
}
