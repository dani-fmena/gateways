using System;
using System.Collections.Generic;

using gateway.domain;

namespace gateway.api.V1.Services.Validators
{
    /// <summary>
    /// Contains business logic validation for object / entity fields 
    /// </summary>
    public static class ValExt
    {
        /// <summary>
        /// Check if the parameter is a proper entity ids value
        /// </summary>
        /// <param name="svc">Service instance</param>
        /// <param name="ids">Entity Id</param>
        /// <remarks>
        /// This is just an example method. It's meant to be used as reference. All the Id checking are done
        /// directly in the controller.
        /// </remarks>
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

        /// <summary>
        /// <para>
        /// First compare if two strong related entity id are the same. It should be used when the
        /// second entity is an extension of the primary entity. E.g items and its specialization. In this kind
        /// of case, this method can act as substitute for <see cref="IsIdValid"/> method. 
        /// </para>
        /// <para>Then if the Ids are good Id number just like <see cref="IsIdValid"/> does</para>
        /// </summary>
        /// <remarks>
        /// If the Id are greater than 0, then they must be the same 'cause StoreData is an extension table
        /// </remarks>
        /// <param name="svc">Service instance</param>
        /// <param name="mainId">Identifier of the main entity / table</param>
        /// <param name="extendedId"></param>
        /// <returns>True if the related identifier are good</returns>
        public static bool AreWellRelated(this SvcEndpointBase svc, int mainId, int extendedId)
        {
            // if the Id are greater than 0, then they must be the same 'cause StoreData is an extension table
            if (mainId > 0 && extendedId > 0 && mainId != extendedId) { svc.AddMemberProblem("ids", MsgAttrVal.MsgMismatchRelatedIds); return false; }
            if (mainId <= 0 || extendedId <= 0) { svc.AddMemberProblem("ids", MsgAttrVal.MsgMbrId); return false; }
                
            return true;
        }

        #region ========= DATA MODEL VALIDATIONS ==============================================
        #endregion ============================================================================
    }
}