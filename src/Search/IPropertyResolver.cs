using System.Linq.Expressions;

namespace Disk.Search
{
    public interface IPropertyResolver
    {
        ParameterExpression ResolveParameter<T>();

        Expression? ResolveProperty<T>(string propertyName, bool allowNesting = false);
    }
}