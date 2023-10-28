using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Abstractions
{
    /// <summary>
    /// This interface implemented primary key entity
    /// </summary>
    /// <typeparam name="TPrimaryKey">
    /// Primary Key type of the entity
    /// </typeparam>
    internal interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        TPrimaryKey Id { get; set; }
    }
}
