using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using gateway.dal;
using gateway.domain;
using gateway.domain.Dto;
using gateway.domain.Entities;
using gateway.api.V1.Services.Validators;


namespace gateway.api.V1.Services
{
    public interface ISvcGateway : ISvcEndpointBase
    {
        public Task<ICollection<DtoGatewayRow>> GetRows();
        public Task<DtoGatewayRow> GetRowById(int gatewayId);
        public Task<Gateway> Create(DtoGatewayIn dtoNewGateway);
        public Task<DtoGatewayIn> Update(DtoGatewayIn dtoNewGateway);
        public Task<bool> BatchDelete(ICollection<int> ids);
    }

    public class SvcGateway : SvcEndpointBase, ISvcGateway
    {
        public SvcGateway(ADbContext dal, IMapper mapper) : base(dal, mapper) { }
        
        /// <summary>Get a specific <see cref="DtoGatewayRow"/> according with the given id</summary>
        /// <param name="gatewayId">Gateway identifier</param>
        /// <returns>A single <see cref="DtoGatewayRow"/></returns>
        public async Task<DtoGatewayRow> GetRowById(int gatewayId)
        {
            try
            {
                var dbGateway = await Dal.Gateways
                        .Include(g => g.Peripherals)
                        .FirstOrDefaultAsync(g => g.Id == gatewayId)
                        .ConfigureAwait(false);

                if (dbGateway != null) return Mapper.Map<Gateway, DtoGatewayRow>(dbGateway);

                // 404
                AddNotFoundProblem($"{DtlProblem.DtlNotFound}");
                return null;               
            }
            catch (Exception e)
            {
                AddProblem($"{e.Message} Origin: {e.Source}");
            }

            return null;
        }

        /// <summary>Get <see cref="Gateway"/>list</summary>
        /// <returns><see cref="Gateway"/> list</returns>
        public async Task<ICollection<DtoGatewayRow>> GetRows()
        {
            try
            {
                var dbGateways = await Dal.Gateways
                    .Include(g => g.Peripherals)
                    .ToListAsync().ConfigureAwait(false);
                
                return Mapper.Map<ICollection<Gateway>, ICollection<DtoGatewayRow>>(dbGateways);;
            }
            catch (Exception e)
            {
                AddProblem($"{e.Message} Origin: {e.Source}");
            }

            return null;
        }

        /// <summary>Insert / creates a new Gateway into the system</summary>
        /// <param name="dtoNewGateway">Gateway's data to post (create/insert)</param>
        /// <returns>A just created<see cref="Gateway"/>, null otherwise</returns>
        public async Task<Gateway> Create(DtoGatewayIn dtoNewGateway)
        {
            try
            {
                var newGateway = Mapper.Map<DtoGatewayIn, Gateway>(dtoNewGateway);

                Dal.Gateways.Add(newGateway);
                await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (newGateway.Id <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                return newGateway;
            }
            catch (DbUpdateException e) { AddMemberProblem(nameof(Gateway.SerialNumber), e.InnerException?.Message); }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }

        /// <summary> Edit a gateway </summary>
        /// <param name="dtoGateway">Gateway's data to update</param>
        /// <returns></returns>
        public async Task<DtoGatewayIn> Update(DtoGatewayIn dtoGateway)
        {
            if (!this.IsIdValid(dtoGateway.Id)) return null;
            try
            {
                var newGateway = Mapper.Map<DtoGatewayIn, Gateway>(dtoGateway);

                Dal.Gateways.Update(newGateway);
                var updatedCount = await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (updatedCount <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                return dtoGateway;
            }
            catch (DbUpdateConcurrencyException) { AddNotFoundProblem(); }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }

        /// <summary>Delete just one gateways or a bunch of them</summary>
        /// <remarks>Note that all the associated peripherals to this gateway will be also removed from the system</remarks>
        /// <param name="ids">List of gateways identifiers to remove</param>
        public async Task<bool> BatchDelete(ICollection<int> ids)
        {
            // business logic validation
            if (ids.Count == 0) { AddMemberProblem(nameof(ids), MsgAttrVal.MsgMbrId); return false; }
            if (!this.IsIdsCollectionValid(ids)) return false;
            
            try
            {
                // checking which of the given ids actually exist on the database
                var existingIds = Dal.Gateways.Where(g => ids.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToList();

                // if no ip exists there is no point in going on, so ... 
                if (existingIds.Count == 0)
                {
                    AddNotFoundProblem();
                    return false;
                }

                // trying to remove then
                Dal.RemoveRange(existingIds.Select(id => new Gateway { Id = id }));
                var removeCount = await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                // checking how it was it
                if (removeCount <= 0)
                {
                    AddNotFoundProblem();
                    return false;
                }
                
                return true;
            }
            catch (Exception e) { AddDalProblem($"{e.Message} Origin: {e.Source}"); }
            return false;
        }
    }
}