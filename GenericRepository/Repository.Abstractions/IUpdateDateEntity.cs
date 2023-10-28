using System;

namespace Repository.Abstractions
{
    /// <summary>
    /// This interface implemented Modification Date property for entity
    /// </summary>
    public interface IUpdateDateEntity
    {
        /// <summary>
        /// Modification Date
        /// </summary>
        public DateTime? ModificationDate { get; set; }
    }
}
