using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using gateway.dal;
using gateway.domain;
using gateway.domain.Dto;
using gateway.domain.Entities;
using gateway.api.V1.Services.Validators;


namespace gateway.api.V1.Services
{
    public interface ISvcPeripherals : ISvcEndpointBase
    {
        public Task<DtoPeripheralRow> Create(DtoPeripheralIn dtoNewGateway);
        public Task<DtoPeripheralRow> GetRowById(int peripheralId);
    }

    public class SvcPeripherals : SvcEndpointBase, ISvcPeripherals
    {
        public SvcPeripherals(ADbContext dal, IMapper mapper) : base(dal, mapper) { }

        /// <summary>Get a specific <see cref="DtoPeripheralRow"/> according with the given id</summary>
        /// <param name="peripheralId">Peripheral identifier</param>
        /// <returns>A single <see cref="DtoPeripheralRow"/></returns>
        public async Task<DtoPeripheralRow> GetRowById(int peripheralId)
        {
            try
            {
                var dbPeripheral = await Dal.Peripherals
                    .FirstOrDefaultAsync(p => p.Id == peripheralId)
                    .ConfigureAwait(false);

                if (dbPeripheral != null) return Mapper.Map<Peripheral, DtoPeripheralRow>(dbPeripheral);

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


        /// <summary>Insert / creates a new Peripheral into the system</summary>
        /// <param name="dtoNewPeripheral">Peripheral's data to post (create/insert)</param>
        /// <returns>A just created<see cref="DtoPeripheralRow"/>, null otherwise</returns>
        public async Task<DtoPeripheralRow> Create(DtoPeripheralIn dtoNewPeripheral)
        {
            if (!await this.IsThereRoomForOneMorePeripheral(dtoNewPeripheral))
            {
                AddProblem(TksProblem.ErrBnssRuleFail, StatusCodes.Status400BadRequest, DtlProblem.MsgMoreThan10Perph);
                return null;
            }

            try
            {
                var newPeripheral = Mapper.Map<DtoPeripheralIn, Peripheral>(dtoNewPeripheral);

                Dal.Peripherals.Add(newPeripheral);
                await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (newPeripheral.Id <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                
                return Mapper.Map<Peripheral, DtoPeripheralRow>(newPeripheral);
                
            }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }
    }
}