using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    public class GreaterThanOrEqualNode : OperatorNode
    {
        public GreaterThanOrEqualNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.GreaterThanOrEqual(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
