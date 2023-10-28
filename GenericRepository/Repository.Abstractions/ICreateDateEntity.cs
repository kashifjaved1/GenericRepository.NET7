using System;

namespace Repository.Abstractions
{
    /// <summary>
    /// This interface implemented creation date for entity
    /// </summary>
    public interface ICreateDateEntity
    {
        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
