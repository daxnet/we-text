
using System;
using System.Linq.Expressions;

namespace WeText.Common.Specifications
{
    /// <summary>
    /// Represents the combined specification which indicates that both of the given
    /// specifications should be satisfied by the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        #region Ctor
        /// <summary>
        /// Constructs a new instance of <c>AndSpecification&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="left">The first specification.</param>
        /// <param name="right">The second specification.</param>
        public AndSpecification(Specification<T> left, Specification<T> right) : base(left, right) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public override Expression<Func<T, bool>> Expression => Left.Expression.And(Right.Expression);
        #endregion
    }

}
