using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    public class OrNode : OperatorNode
    {
        public OrNode(IQueryNode left, IQueryNode right) : base(left, right)
        {

        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.OrElse(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
