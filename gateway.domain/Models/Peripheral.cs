using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gateway.domain.Models
{
    public class Peripheral
    {
        public int Id { get; set; }
        
        /// <summary>UID device identifier</summary> 
        public Guid Uid { get; set; }
        
        /// <summary>A unique serial number</summary>
        [Required, Column(TypeName = "NVARCHAR(36)")]
        public string Vendor { get; set; }
        
        /// <summary>Is the device online </summary>
        public bool IsOnline { get; set; }

        /// <summary>Gateway that owns the peripheral</summary>
        public Gateway Gateway { get; set; }
        
        /// <summary>Creation date</summary>
        public DateTime Created { get; set; }
    }
}