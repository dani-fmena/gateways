using System;
using AutoMapper;

using gateway.domain.Dto;
using gateway.domain.Entities;

namespace gateway.api.V1.Mapping
{
    /// <summary>DTOs to entities and vice versa mapping profile</summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            AllowNullCollections = false;
                
            #region ======= GATEWAY ==================================================================

            CreateMap<Gateway, DtoGatewayRow>()
                .ForMember(
                    dto => dto.PeripheralsAssociated,
                    option => option.MapFrom( src => src.Peripherals.Count));
            
            CreateMap<DtoGatewayIn, Gateway>()
                .ForMember(entity => entity.Peripherals, option => option.Ignore());


            #endregion ================================================================================
            
            #region ======= PERIPHERALS ===============================================================

            CreateMap<DtoPeripheralIn, Peripheral>()
                .ForMember(p => p.Uid, option => option.MapFrom(src => Guid.NewGuid()));

            CreateMap<Peripheral, DtoPeripheralRow>();


            #endregion ================================================================================
        }
    }
}