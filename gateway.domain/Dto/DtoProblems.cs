using System.Collections.Generic;

namespace gateway.domain.Dto
{
    /// <summary>
    /// Struct with the info when related with problem may occurs in execution.
    /// Partially rfc7807 compliant
    /// 
    /// See http://tools.ietf.org/html/rfc7807"
    /// </summary>
    public class Problem
    {
        /// <summary>HTTP status error code</summary>
        /// <example>403</example>
        public int Status { get; set; }
        
        /// <summary>Token that identify the error type</summary>
        /// <example>Err.Forbidden</example>
        public string Type { get; set; }
        
        /// <summary>Details of the error (only in debug mode)</summary>
        /// <example>You has no permission to perform the operation</example>
        public string Title { get; set; }

        /// <summary>
        /// Struct with the info related with member errors. E.g validation errors. 
        /// </summary>
        /// <summary>Problem member's error dictionary</summary>
        /// <remarks>
        /// The <c>Key</c> is the model / dto / object member. The <c>List</c> contains the member errors 
        /// Could be null or have 0 items in some cases in which the other <see cref="Problem"/> property
        /// are more important.
        /// </remarks>
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}