using Mandalium.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Infrastructure.Specifications
{
    public class GenericSpecification<T> : BaseSpecification<T> where T : class
    {
        public GenericSpecification(bool addIncludes, Expression<Func<T, object>> includeExpression, Expression<Func<T, bool>> criteria = null) : base(criteria)
        {
            if (addIncludes)
            {
                AddInclude(includeExpression);
            }
        }


        public GenericSpecification(bool addIncludes, List<Expression<Func<T, object>>> includeExpressionList, Expression<Func<T, bool>> criteria) : base (criteria)
        {
            if (addIncludes)
            {
                foreach (var exp in includeExpressionList)
                {
                    AddInclude(exp);
                }
            }
        }
    }
}
