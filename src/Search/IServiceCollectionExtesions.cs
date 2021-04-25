using Disk.Search.Query;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disk.Search
{
    public static class IServiceCollectionExtesions
    {
        public static void AddExpressionParser<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddTransient<IExpressionParser, ExpressionParser>();
            services.AddTransient<IQueryParser, QueryParser>();
            services.AddTransient<IPropertyResolver, DbContextPropertyResolver<T>>();
        }
    }
}
