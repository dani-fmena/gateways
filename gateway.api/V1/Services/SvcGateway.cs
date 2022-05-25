using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using gateway.dal;
using gateway.domain.Dto;
using gateway.domain.Entities;


namespace gateway.api.V1.Services
{
    public interface ISvcGateway : ISvcEndpointBase
    {
        public Task<ICollection<DtoGatewayRow>> GetGatewaysRows();
    }

    public class SvcGateway : SvcEndpointBase, ISvcGateway
    {
        public SvcGateway(ADbContext dal, IMapper mapper) : base(dal, mapper)
        {
        }

        /// <summary>Get all the data that defines a <see cref="Gateway"/>, root locations included</summary>
        /// <returns><see cref="Gateway"/> data</returns>
        public async Task<ICollection<DtoGatewayRow>> GetGatewaysRows()
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
    }
}