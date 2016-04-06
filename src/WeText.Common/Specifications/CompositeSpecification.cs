

namespace WeText.Common.Specifications
{
    /// <summary>
    /// Represents the base class for composite specifications.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
    {
        #region Private Fields
        private readonly Specification<T> left;
        private readonly Specification<T> right;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs a new instance of <c>CompositeSpecification&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="left">The first specification.</param>
        /// <param name="right">The second specification.</param>
        public CompositeSpecification(Specification<T> left, Specification<T> right)
        {
            this.left = left;
            this.right = right;
        }
        #endregion

        #region ICompositeSpecification Members
        /// <summary>
        /// Gets the first specification.
        /// </summary>
        public Specification<T> Left
        {
            get { return this.left; }
        }
        /// <summary>
        /// Gets the second specification.
        /// </summary>
        public Specification<T> Right
        {
            get { return this.right; }
        }
        #endregion
    }
}
