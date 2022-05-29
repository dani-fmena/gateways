using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public interface ISvcPeripherals : ISvcEndpointBase
    {
        public Task<DtoPeripheralRow> Create(DtoPeripheralIn dtoNewPeripheral);
        public Task<DtoPeripheralRow> Update(DtoPeripheralUpdateIn dtoNewPeripheral);
        public Task<DtoPeripheralRow> GetRowById(int peripheralId);
        public Task<bool> BatchDelete(ICollection<int> ids);
        Task<ICollection<DtoPeripheralRow>> GetRowsByGateway(int gatewayId);
        Task<ICollection<DtoPeripheralRow>> GetRows();
    }

    public class SvcPeripherals : SvcEndpointBase, ISvcPeripherals
    {
        public SvcPeripherals(ADbContext dal, IMapper mapper) : base(dal, mapper) { }
        
        /// <summary>Get <see cref="Peripheral"/>list</summary>
        /// <returns><see cref="Peripheral"/> list</returns>
        public async Task<ICollection<DtoPeripheralRow>> GetRows()
        {
            try
            {
                var dbPeripherals = await Dal.Peripherals
                    .ToListAsync().ConfigureAwait(false);
                
                return Mapper.Map<ICollection<Peripheral>, ICollection<DtoPeripheralRow>>(dbPeripherals);;
            }
            catch (Exception e)
            {
                AddProblem($"{e.Message} Origin: {e.Source}");
            }

            return null;
        }
        
        /// <summary>Get <see cref="Peripheral"/>list</summary>
        /// <remarks>The data will be filtered by the given <see cref="Gateway"/> identifier</remarks>
        /// <param name="gatewayId"><see cref="Gateway"/> that owns the peripherals wants to be listed</param>
        /// <returns><see cref="Peripheral"/> list</returns>
        public async Task<ICollection<DtoPeripheralRow>> GetRowsByGateway(int gatewayId)
        {
            try
            {
                var dbPeripherals = await Dal.Peripherals
                    .Where(p => p.GatewayId == gatewayId)
                    .ToListAsync().ConfigureAwait(false);
                
                return Mapper.Map<ICollection<Peripheral>, ICollection<DtoPeripheralRow>>(dbPeripherals);;
            }
            catch (Exception e)
            {
                AddProblem($"{e.Message} Origin: {e.Source}");
            }

            return null;
        }

        public async Task<DtoPeripheralRow> Update(DtoPeripheralUpdateIn dtoNewPeripheral)
        {
            var newPeripheral = Mapper.Map<DtoPeripheralUpdateIn, Peripheral>(dtoNewPeripheral);

            // business validations 
            if (!this.IsIdValid(dtoNewPeripheral.Id)) return null;
            if (!await this.IsThereRoomForOneMorePeripheral(newPeripheral))
            {
                AddProblem(TksProblem.ErrBnssRuleFail, StatusCodes.Status400BadRequest, DtlProblem.MsgMoreThan10Perph);
                return null;
            }
            
            try
            {
                Dal.Peripherals.Update(newPeripheral);
                var updatedCount = await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (updatedCount <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                
                return Mapper.Map<Peripheral, DtoPeripheralRow>(newPeripheral);;
            }
            catch (DbUpdateConcurrencyException) { AddNotFoundProblem(); }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }

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
            var newPeripheral = Mapper.Map<DtoPeripheralIn, Peripheral>(dtoNewPeripheral);
            
            // business validations
            if (!await this.IsThereRoomForOneMorePeripheral(newPeripheral))
            {
                AddProblem(TksProblem.ErrBnssRuleFail, StatusCodes.Status400BadRequest, DtlProblem.MsgMoreThan10Perph);
                return null;
            }

            try
            {
                Dal.Peripherals.Add(newPeripheral);
                await Dal.SaveChangesAsync().ConfigureAwait(false);
                
                if (newPeripheral.Id <= 0) AddDalProblem(DtlProblem.DtlOpsNotSuccessful);
                
                return Mapper.Map<Peripheral, DtoPeripheralRow>(newPeripheral);
                
            }
            catch (Exception e) { AddProblem($"{e.Message} Origin: {e.Source}"); }
            return null;
        }

        /// <summary>Delete just one peripheral or a bunch of them</summary>
        /// <param name="ids">List of peripheral identifiers to remove</param>
        public async Task<bool> BatchDelete(ICollection<int> ids)
        {
            // business logic validation
            if (ids.Count == 0) { AddMemberProblem(nameof(ids), MsgAttrVal.MsgMbrId); return false; }
            if (!this.IsIdsCollectionValid(ids)) return false;
            
            try
            {
                // checking which of the given ids actually exist on the database
                var existingIds = Dal.Peripherals.Where(g => ids.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToList();

                // if no ip exists there is no point in going on, so ... 
                if (existingIds.Count == 0)
                {
                    AddNotFoundProblem();
                    return false;
                }

                // trying to remove then
                Dal.RemoveRange(existingIds.Select(id => new Peripheral { Id = id }));
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