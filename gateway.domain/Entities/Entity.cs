using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gateway.domain.Entities
{
    /// <summary>
    /// Base entity general class for shape up other entities with the <c>ID</c>.
    /// </summary>
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
    
    /// <summary>
    /// Same class as <see cref="Entity"/> but has the basic date field of created
    /// </summary>
    /// <remarks>
    /// D == with dates (just creation date int this particular case)
    /// We are going to work with UTC at system level and Tz only in the UI (front) level
    /// </remarks>
    public class EntityD : Entity
    {
        /// <summary>Creation date</summary>
        /// <example>2022-05-28 22:27:55</example>
        public DateTime Created { get; set; }
    }
}