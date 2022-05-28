using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace gateway.domain.Dto
{
    /// <summary>Transfer object to post / create new gateway</summary>
    public class DtoPeripheralIn
    {
        /// <summary>Gateway database identifier</summary>
        /// <example>15</example> 
        [AllowNull, Range(0, UInt32.MaxValue)]
        public int Id { get; set; }
        
        /// <summary>Vendor name</summary>
        /// <example>Macintosh</example>
        [Required, MaxLength(36)]
        public string Vendor { get; set; }
        
        /// <summary>Is the device online </summary>
        /// <example>true</example>
        public bool IsOnline { get; set; }

        /// <summary>Gateway identifier that owns the peripheral</summary>
        /// <example>5</example>
        [Required, Range(0, UInt32.MaxValue)]
        public int GatewayId { get; set; }
    }
    
    /// <summary>
    /// Transfer object (ViewModel) good to represent a row in a list / table of peripherals.
    /// </summary>
    public class DtoPeripheralRow
    {
        /// <summary>Gateway database identifier</summary>
        /// <example>15</example>
        public int Id { get; set; }
        
        /// <summary>UID device identifier</summary>
        /// <example>DE303349-5506-B943-04BA-2CE17043613A</example>
        public Guid Uid { get; set; }
        
        /// <summary>Vendor name</summary>
        /// <example>Macintosh</example>
        public string Vendor { get; set; }
        
        /// <summary>Is the device online </summary>
        /// <example>true</example>
        public bool IsOnline { get; set; }

        /// <summary>Gateway identifier that owns the peripheral</summary>
        /// <example>5</example>
        public int GatewayId { get; set; }
        
        /// <summary>Creation date</summary>
        /// <example>2022-05-28 22:27:55</example>
        public DateTime Created { get; set; }
    }
}