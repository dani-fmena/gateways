namespace gateway.domain
{
    /// <summary>
    /// Token problem cause identification
    /// </summary>
    public static class TksProblem
    {
        /// <summary>When the param given to the endpoint was invalid for some reason</summary>
        public const string ErrInvEntity = "Err.Invalid.Entity";
        public const string ErrDal = "Err.DAL";
        public const string ErrResNotFound = "Err.Resource.NotFound";
        public const string ErrNullProbType = "Err.Null.Problem.Type";
        public const string ErrBnssRuleFail = "Err.Bnss.Rule.Fail";
    }

    /// <summary>
    /// Problem details / descriptions static definitions
    /// </summary>
    public static class DtlProblem
    {
        public const string DtlInvalidEntityData = "Invalid entity data was given.";
        public const string DtlProblemTypeNotNull = "The problem (error) type property can't be null.";
        public const string DtlNotFound = "The requested resource(s) wasn't found.";
        public const string DtlOpsNotSuccessful = "The operation was not totally successful, but it did not fail either. Please check the inbound data.";
        public const string MsgMoreThan10Perph = "The gateway has more than 10 peripherals or the gateway doesn't exist.";
    }

    /// <summary>
    /// Error messages definition for custom attribute / field validators
    /// </summary>
    public static class MsgAttrVal
    {
        public const string MsgMbrId = "Entity ideintifier was invalid according to the requested operation.";
        public const string MsgInvalidString = "The string don't seems to be valid.";
        public const string MsgInvalidIpv4 = "The given IPv4 don't seems to be valid.";
    }
}