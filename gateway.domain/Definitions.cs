namespace gateway.domain
{
    /// <summary> Documentation (swagger) tags </summary>
    public static class EndpointTags
    {
        // Entities
        public const string TgGateways = "Admin | Gateways";                    // Tg = tag
        public const string TgPeripherals = "Admin | Peripherals";
        
        // ...
    }
    
    /// <summary>
    /// Regular Expression patterns shared across the project 
    /// </summary>
    public static class RegxPatterns
    {
        public const string Alphanumeric = @"^\w*$";
        public const string IPv4 = @"^((25[0-5]|(2[0-4]|1[0-9]|[1-9]|)[0-9])(\.(?!$)|$)){4}$";
    }
}