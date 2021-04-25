using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    public class EqualNode : OperatorNode
    {
        public EqualNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Equal(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
