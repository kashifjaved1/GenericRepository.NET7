using Ardalis.Specification;
using GenericRepository.Demo.Entities;
using GenericRepository.Demo.Specs;

namespace GenericRepository.Demo.Specs
{
    /// <summary>
    /// Order by author name specification
    /// </summary>
    public sealed class AuthorOrderByNameSpec : Specification<Author>
    {
        /// <inheritdoc />
        public AuthorOrderByNameSpec(string name)
        {
            Query.ApplyBaseRules().ApplyByName(name).OrderBy(o => o.Name);
        }
    }

    public static class AuthorSpecification
    {
        public static ISpecificationBuilder<Author> ApplyBaseRules(
            this ISpecificationBuilder<Author> specificationBuilder)
        {
            specificationBuilder.Include(x => x.Books).Include(x => x.Articles);

            return specificationBuilder;
        }

        public static ISpecificationBuilder<Author> ApplyByName(
            this ISpecificationBuilder<Author> specificationBuilder, string name)
        {
            specificationBuilder.Where(a => a.Name.Contains(name));

            return specificationBuilder;
        }
    }
}