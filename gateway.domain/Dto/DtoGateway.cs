using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace gateway.domain.Dto
{
    /// <summary>Transfer object (ViewModel) good to represent a row in a list / table of Gateways</summary>
    public class DtoGatewayRow
    {
        /// <summary>Gateway database identifier</summary>
        public int Id { get; set; }
        
        /// <summary>Human-readable name</summary>
        /// <example>Office</example>
        public string Name { get; set; }
        
        /// <summary>A unique serial number</summary>
        /// <example>ABC789</example>
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4 / IPv6 Address</summary>
        /// <example>127.0.0.1</example>
        public string IpAddress { get; set; }
        
        /// <summary>Count of associated peripheral to this gateway</summary>
        public uint PeripheralsAssociated { get; set; }
    }
    
    /// <summary>Transfer object to post / create new gateway</summary>
    public class DtoGatewayIn
    {
        /// <summary>Gateway database identifier</summary>
        /// <example>15</example> 
        [AllowNull, Range(0, UInt32.MaxValue)]
        public int Id { get; set; }
        
        /// <summary>Human-readable name</summary>
        /// <example>Office</example>
        [Required, MaxLength(36)]
        public string Name { get; set; }

        /// <summary>A unique serial number</summary>
        /// <example>ABC789</example>>
        [Required, Alphanumeric, MaxLength(16)]
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4 / IPv6 Address</summary>
        /// <example>127.0.0.1</example>
        [Required, IPv4]
        public string IpAddress { get; set; }
    }
}