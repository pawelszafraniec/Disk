using System;
using System.Linq.Expressions;

namespace Disk.Search.Query
{
    public interface IQuery
    {
        IQueryNode Root { get; }

        Expression<Func<T, bool>> ToExpression<T>(IPropertyResolver propertyResolver);
    }
}