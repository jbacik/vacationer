using backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace backend.integrationtests
{
    public class SliceFixture
    {
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static SliceFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();
            services.AddLogging();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();

            var dbBuilder = new DbContextOptionsBuilder();
            dbBuilder.UseInMemoryDatabase("VacationTime-Tests");
        }

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<VacationContext>();

                await action(scope.ServiceProvider).ConfigureAwait(false);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<VacationContext>();
                
                var result = await action(scope.ServiceProvider).ConfigureAwait(false);
                await dbContext.SaveChangesAsync();

                return result;
            }
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<VacationContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<VacationContext>()));

        public static Task ExecuteDbContextAsync(Func<VacationContext, IMediator, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetService<VacationContext>(), sp.GetService<IMediator>()));

        public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<T>(params T[] entities) where T : class
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Set<T>().Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }
        
        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}
