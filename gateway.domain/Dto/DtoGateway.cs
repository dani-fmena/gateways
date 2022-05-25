
namespace gateway.domain.Dto
{
    /// <summary>Transfer object (ViewModel) good to create a list / table of gateways</summary>
    public class DtoGatewayRow
    {
        /// <summary>Gateway database identifier</summary>
        public int Id { get; set; }
        
        /// <summary>Human-readable name</summary>
        public string Name { get; set; }
        
        /// <summary>A unique serial number</summary>
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4 / IPv6 Address</summary>
        public string IpAddress { get; set; }
        
        /// <summary>Count of associated peripheral to this gateway</summary>
        public uint PeripheralsAssociated { get; set; } 
    }
}