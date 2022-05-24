using System.Net;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gateway.domain.Models
{
    /// <summary>Gateway <b>entity</b> to control 10 peripheral devices</summary>
    public class Gateway
    {
        
        public int Id { get; set; }

        /// <summary>Human-readable name</summary>
        [Required, Column(TypeName = "NVARCHAR(36)")]
        public string Name { get; set; }

        /// <summary>A unique serial number</summary>
        [Required, Column(TypeName = "NVARCHAR(16)")]
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4 / IPv6 Address</summary>
        [Required, MinLength(4), MaxLength(16)]
        public byte[] IpAddressBytes { get; set; }

        /// <summary>Multiple associated peripheral devices</summary>
        public ICollection<Peripheral> Peripherals { get; set; }
        
        [NotMapped]
        public IPAddress IpAddress
        {
            get => new IPAddress(IpAddressBytes);
            set => IpAddressBytes = value.GetAddressBytes();
        }
    }
}