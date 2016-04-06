
using System;
using System.Linq.Expressions;

namespace WeText.Common.Specifications
{
    /// <summary>
    /// Represents the specification which indicates the semantics opposite to the given specification.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public class NotSpecification<T> : Specification<T>
    {
        #region Private Fields
        private Specification<T> spec;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>NotSpecification&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="specification">The specification to be reversed.</param>
        public NotSpecification(Specification<T> specification)
        {
            this.spec = specification;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public override Expression<Func<T, bool>> Expression => this.spec.Expression.Not();
        #endregion
    }
}
