using Ardalis.Specification;
using GenericRepository.Demo.Entities;

namespace GenericRepository.Demo.Specs
{
    /// <summary>
    /// Author By Name specification
    /// </summary>
    public sealed class AuthorByNameSpec : Specification<Author>
    {
        /// <inheritdoc />
        public AuthorByNameSpec(string name)
        {
            Query.Where(c => c.Name == name);
        }
    }
}