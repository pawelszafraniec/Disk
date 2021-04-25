using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    public class LessThanNode : OperatorNode
    {
        public LessThanNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.LessThan(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
