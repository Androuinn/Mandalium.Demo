using Mandalium.Core.Persisence.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mandalium.Infrastructure.Specifications
{
    public class GenericSpecification<T> : BaseSpecification<T> where T : class
    {
        public GenericSpecification(Expression<Func<T, bool>> criteria = null) : base(criteria)
        {

        }

        public GenericSpecification(bool addIncludes, Expression<Func<T, object>> includeExpression, Expression<Func<T, bool>> criteria = null) : base(criteria)
        {
            if (addIncludes)
            {
                AddInclude(includeExpression);
            }
        }


        public GenericSpecification(bool addIncludes, List<Expression<Func<T, object>>> includeExpressionList, Expression<Func<T, bool>> criteria) : base(criteria)
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
