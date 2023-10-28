using Repository.Abstractions;

namespace GenericRepository.Demo.Entities
{
    public class Book : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public int TotalPage { get; set; }

        public Guid AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}
