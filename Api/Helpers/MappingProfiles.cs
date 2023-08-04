using Api.Dtos;
using AutoMapper;
using Core.Entities;

namespace Api.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<TaxReturn , TaxReturnDtoReponse>().ReverseMap();
            CreateMap<TaxReturn, TaxReturnDto>().ReverseMap();
            CreateMap<TaxHistory, TaxHistoryDtoReponse>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();

        }
    }
}
