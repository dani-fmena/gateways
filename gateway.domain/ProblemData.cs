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
        public const string ErrDalDuplicateKey = "Err.Dal.DuplicateKey";
        public const string ErrResNotFound = "Err.Resource.NotFound";
        public const string ErrNotFoundOrDeleted = "Err.NotFound.Or.Deleted";            // The deleted if for entity with soft delete field
        public const string ErrNullProbType = "Err.Null.Problem.Type";
        public const string ErrSoftDelete = "Err.Soft.Delete";
        public const string ErrBadArgs = "Err.Bad.Arguments";
    }

    /// <summary>
    /// Problem details / descriptions static definitions
    /// </summary>
    public static class DtlProblem
    {
        public const string DtlInvalidEntityData = "Invalid entity data was given.";
        public const string DtlProblemTypeNotNull = "The problem (error) type property can't be null.";
        public const string DtlNotFound = "The requested resource(s) wasn't found.";
        public const string DtlUndefinedEntity = "The entity type was undefined, so table can't be found.";
        public const string DtlMissingDeleteField = "The entity, object or table is non 'soft delete' friendly.";
        public const string NotFoundDeletedException = "One or more entities wasn't found or was deleted already.";
        public const string DtlSoftDeleted = "The entity is no more.";
        public const string DtlInvalidArgsCount = "Wrong args count. Expeted {0} args, {1} given.";
        public const string DtlStaffNeedRole = "A role has to be specified for staff members.";
        public const string DtlDalDuplicateKey = "A key seems duplicated. Constraint is failing with \'{0}\'.";
        public const string DtlOpsNotSuccessful = "The operation was not totally successful, but it did not fail either. Please check the inbound data.";
        public const string DtlBulkOpsNotSuccessful = "The operation was not totally successful (but data was not modified at all). Please check the inbound data.";
        public const string DtlOpsSaveFail = "Data wasn't save in to the database.";
        public const string DtlCollectionsToBig = "The collection is to big. \'{0}\'";
        public const string DtlCollectionsSizeMissMatch = "There is a miss match in the collections size, must be the same";
        public const string DtlDatabaseTableMapNotFound = "No map was found for the given field, a map must exist";
    }

    /// <summary>
    /// Error messages definition for custom attribute / field validators
    /// </summary>
    public static class MsgAttrVal
    {
        public const string MsgMbrId = "Entity ideintifier was invalid according to the requested operation.";
        public const string MsgOutdated = "Entity is out of date, please sync.";
        public const string MsgValNotExpected = "The given value isn't expected.";
        public const string MsgStaffWrongEnv = "Enviroment not allowed for staff.";
        public const string MsgInvalidStoreType = "The given store type don't seems to be valid.";
        public const string MsgInvalidAllergen = "The given allergens wasn't valid.";
        public const string MsgInvalidRange = "The length of the field needs to be between {0} and {1}.";
        public const string MsgMismatchRelatedIds = "Mismatch found with strong related entity identifier";
        public const string MsgInvalidString = "The string don't seems to be valid.";
        public const string MsgOnlyCapsLetters = "Only caps letters allowed.";
        public const string MsgInvalidIpv4 = "The given IPv4 don't seems to be valid.";
        public const string MsgInvalidOrder = "Invalid order direction.";
        public const string MsgInvalidRelations = "The related entities don't seems to be valid.";
        public const string MsgInvalidTimeRangeEnd = "The end of schedule timerange most be grater than the start value.";
        public const string MsgNotNullValuesAllowed = "The collection can't contains null values.";
        public const string MsgMinLength = "{0} must be greater or equal than {1}.";
        public const string MsgConditionalRequirement = "This property is mandatory when {0} is {1}.";
        public const string MsgInvalidFilters = "The given filter '{0}' don't seems valid.";
        public const string MsgInvalidPriceLevel = "The store price level isn't valid. The values given must be between 1 ~ 3 (inclusive).";
        public const string MsgInvalidReadyTime = "The given store ready time value in minutes isn't valid.";
        public const string MsgInvalidGeoLocation = "The given value don't seem as a geo location coordinate.";
        public const string MsgInvalidTimeRange = "Invalid value. It must be in terms of 24hrs with round numbers, or 59 minutes as Top value.";
    }
}