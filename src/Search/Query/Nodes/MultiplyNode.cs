using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    public class MultiplyNode : OperatorNode
    {
        public MultiplyNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Multiply(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
