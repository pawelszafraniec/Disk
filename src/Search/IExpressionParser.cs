using System;
using System.Linq.Expressions;

namespace Disk.Search
{
    public interface IExpressionParser
    {
        Expression<Func<T, bool>> ParseExpression<T>(string expression);
    }
}