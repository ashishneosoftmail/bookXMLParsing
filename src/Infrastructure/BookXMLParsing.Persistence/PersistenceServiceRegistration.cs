using BookXMLParsing.Application.Contracts.Persistence;
using BookXMLParsing.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookXMLParsing.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddInterfaceServices(
           this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }
    }
}
