using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using gateway.domain.Entities;

namespace gateway.domain.Entities
{
    public class Peripheral : EntityD
    {
        /// <summary>UID device identifier</summary> 
        public Guid Uid { get; set; }
        
        /// <summary>A unique serial number</summary>
        [Required, Column(TypeName = "NVARCHAR(36)")]
        public string Vendor { get; set; }
        
        /// <summary>Is the device online </summary>
        public bool IsOnline { get; set; }

        /// <summary>Gateway that owns the peripheral</summary>
        public Gateway Gateway { get; set; }
    }
}