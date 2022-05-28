using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using gateway.domain.Entities;

namespace gateway.domain.Entities
{
    public class Peripheral : EntityD
    {
        /// <summary>UID device identifier</summary>
        /// <example>DE303349-5506-B943-04BA-2CE17043613A</example> 
        public Guid Uid { get; set; }
        
        /// <summary>Vendor name</summary>
        /// <example>Macintosh</example>
        [Required, Column(TypeName = "NVARCHAR(36)")]
        public string Vendor { get; set; }
        
        /// <summary>Is the device online </summary>
        /// <example>true</example>
        public bool IsOnline { get; set; }
        
        /// <summary>Gateway identifier that owns the peripheral</summary>
        /// <example>5</example>
        public int GatewayId { get; set; }

        /// <summary>Gateway that owns the peripheral</summary>
        /// <example><see cref="Gateway"/></example>
        public Gateway Gateway { get; set; }
    }
}