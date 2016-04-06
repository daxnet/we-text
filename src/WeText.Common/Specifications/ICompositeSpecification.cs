
namespace WeText.Common.Specifications
{
    /// <summary>
    /// Represents that the implemented classes are composite specifications.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public interface ICompositeSpecification<T>
    {
        /// <summary>
        /// Gets the left side of the specification.
        /// </summary>
        Specification<T> Left { get; }
        /// <summary>
        /// Gets the right side of the specification.
        /// </summary>
        Specification<T> Right { get; }
    }
}
