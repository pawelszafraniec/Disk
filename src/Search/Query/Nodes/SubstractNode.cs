using System.Linq.Expressions;

namespace Disk.Search.Query.Nodes
{
    class SubstractNode : OperatorNode
    {
        public SubstractNode(IQueryNode left, IQueryNode right) : base(left, right)
        {
        }

        public override Expression ToExpression<T>(IPropertyResolver propertyResolver)
        {
            return Expression.Subtract(this.Left.ToExpression<T>(propertyResolver), this.Right.ToExpression<T>(propertyResolver));
        }
    }
}
