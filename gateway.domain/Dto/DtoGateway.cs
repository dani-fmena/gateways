
namespace gateway.domain.Dto
{
    /// <summary>Transfer object (ViewModel) good to represent a row in a list / table of Gateways</summary>
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
    
    /// <summary>Transfer object to post / create new gateway</summary>
    public class DtoGatewayIn
    {
        /// <summary>Gateway database identifier</summary>
        public int Id { get; set; }
        
        /// <summary>Human-readable name</summary>
        public string Name { get; set; }
        
        /// <summary>A unique serial number</summary>
        public string SerialNumber { get; set; }
        
        /// <summary>TCP IPv4 / IPv6 Address</summary>
        public string IpAddress { get; set; }
    }
}