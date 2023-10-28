using Repository.Abstractions;
using System.Text.Json.Serialization;

namespace GenericRepository.Demo.Entities
{
    public class Author : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
