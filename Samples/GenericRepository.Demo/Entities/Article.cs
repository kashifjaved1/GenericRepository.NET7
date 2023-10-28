using Repository.Abstractions;

namespace GenericRepository.Demo.Entities
{
    public class Article : BaseEntity<Guid>
    {
        public string Topic { get; set; }
        public string Content { get; set; }

        public Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}
