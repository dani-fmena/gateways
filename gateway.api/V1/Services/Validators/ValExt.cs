using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using gateway.domain;
using gateway.domain.Dto;
using gateway.domain.Entities;

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
        
        /// <summary>
        /// Check if there is room to associate this new <see cref="Peripheral"/> to an existing <see cref="Gateway"/>.
        /// According with the business rules, a Gateway can only have 10 peripheral associated, tops. 
        /// </summary>
        /// <param name="svc">Service instance</param>
        /// <param name="dtoNewPeripheral">new <see cref="DtoPeripheralIn"/>post data</param>
        /// <returns>False if the check fails</returns>
        public static async Task<bool> IsThereRoomForOneMorePeripheral(this SvcEndpointBase svc, DtoPeripheralIn dtoNewPeripheral)
        {
            var dbGateway = await svc.Dal.Gateways
                .Include(g => g.Peripherals)
                .FirstOrDefaultAsync(g => g.Id == dtoNewPeripheral.GatewayId)
                .ConfigureAwait(false);

            if (dbGateway == null) return false;
            return dbGateway.Peripherals.Count < 10;
        }
        
        #endregion ============================================================================
    }
}