using System;
using System.Collections.Generic;

using gateway.domain;

namespace gateway.api.V1.Services.Validators
{
    /// <summary>
    /// Contains business logic validation and a basic data model validation 
    /// </summary>
    public static class ValExt
    {
        #region ========= DATA MODEL VALIDATIONS ==============================================
        
        /// <summary>
        /// Check if the parameter is a proper entity ids value for updates operation, in which id can't be
        ///  less than zero
        /// </summary>
        /// <param name="svc">Service instance</param>
        /// <param name="ids">Entity Id</param>
        /// <returns>False if the check fails</returns>
        public static bool IsIdValid(this SvcEndpointBase svc, params int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
                if (ids[i] <= 0)
                {
                    svc.AddMemberProblem(nameof(ids), MsgAttrVal.MsgMbrId);
                    return false;
                }
            
            return true;
        }
        
        /// <summary>
        /// Check is all the given identifiers are valid
        /// </summary>
        /// <param name="svc">Service instance</param>
        /// <param name="ids">collection of identifiers to check</param>
        /// <returns>False if the check fails</returns>
        public static bool IsIdsCollectionValid(this SvcEndpointBase svc, ICollection<int> ids)
        {
            foreach (var id in ids)
                if (id <= 0)
                {
                    svc.AddMemberProblem(nameof(id), MsgAttrVal.MsgMbrId);
                    return false;
                }

            return true;
        }
        
        #endregion ============================================================================
        
        #region ========= BUSINESS RULES VALIDATIONS ==========================================
        #endregion ============================================================================
    }
}