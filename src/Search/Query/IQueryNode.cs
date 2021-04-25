using System.Linq.Expressions;

namespace Disk.Search.Query
{
    public interface IQueryNode
    {
        bool IsConstant { get; }

        Expression ToExpression<T>(IPropertyResolver propertyResolver);
    }
}
