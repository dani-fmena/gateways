using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gateway.domain.Entities
{
    /// <summary>Gateway <b>entity</b> to control 10 peripheral devices</summary>
    public class Gateway : Entity
    {
        /// <summary>Human-readable name</summary>
        [Required, Column(TypeName = "NVARCHAR(36)")]
        public string Name { get; set; }

        /// <summary>A unique serial number</summary>
        [Required, Column(TypeName = "NVARCHAR(16)")]
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4</summary>
        [Required, MinLength(8), MaxLength(15)]
        public string IpAddress { get; set; }

        /// <summary>Multiple associated peripheral devices</summary>
        public ICollection<Peripheral> Peripherals { get; set; }
    }
}