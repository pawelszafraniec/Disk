using System;
using System.Linq.Expressions;

namespace Disk.Search
{
    public interface IProperty
    {
        Expression Expression { get; }

        Type ReturnType { get; }
    }
}