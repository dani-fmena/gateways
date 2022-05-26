using System.Net;
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
            AllowNullCollections = false;               // https://docs.automapper.org/en/latest/Lists-and-arrays.html?highlight=collection
                
            #region ======= GATEWAY ==================================================================

            CreateMap<Gateway, DtoGatewayRow>()
                .ForMember(
                    dto => dto.PeripheralsAssociated,
                    option => option.MapFrom( src => src.Peripherals.Count));
            
            CreateMap<DtoGatewayIn, Gateway>()
                .ForMember(entity => entity.Peripherals, option => option.Ignore());


            #endregion ================================================================================
        }
    }
}