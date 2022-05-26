using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using gateway.dal;
using gateway.domain;
using gateway.domain.Dto;
using gateway.domain.Entities;


namespace gateway.api.V1.Services
{
    public interface ISvcGateway : ISvcEndpointBase
    {
        public Task<ICollection<DtoGatewayRow>> GetRows();
        public Task<DtoGatewayRow> GetRowById(int gatewayId);
        public Task<Gateway> CreateGateway(DtoGatewayIn dtoNewGateway);
    }

    public class SvcGateway : SvcEndpointBase, ISvcGateway
    {
        public SvcGateway(ADbContext dal, IMapper mapper) : base(dal, mapper) { }
        
        public async Task<DtoGatewayRow> GetRowById(int gatewayId)
        {
            try
            {
                var dbGateway = await Dal.Gateways
                        .Include(g => g.Peripherals)
                        .FirstAsync(g => g.Id == gatewayId)
                        .ConfigureAwait(false);
                    
                return Mapper.Map<Gateway, DtoGatewayRow>(dbGateway);
            }
            catch (Exception e)
            {
                AddProblem($"{e.Message} Origin: {e.Source}");
            }

            return null;
        }

        /// <summary>Get all the data that defines a <see cref="Gateway"/>, root locations included</summary>
        /// <returns><see cref="Gateway"/> data</returns>
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
        /// <returns>The id of the new gateway, 0 otherwise</returns>
        public async Task<Gateway> CreateGateway(DtoGatewayIn dtoNewGateway)
        {
            try
            {
                var newGateway = Mapper.Map<DtoGatewayIn, Gateway>(dtoNewGateway);

                Dal.Gateways.Add(newGateway);
                await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (newGateway.Id <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                return newGateway;
            }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }
    }
}