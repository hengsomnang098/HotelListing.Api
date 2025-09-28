using AutoMapper;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Country;
using HotelListing.Api.DTOs.Hotel;

namespace HotelListing.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Hotel, GetHotelDto>()
        .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name));
        CreateMap<CreateHotelDto, Hotel>();
        CreateMap<UpdateHotelDto, Hotel>();
        CreateMap<Hotel, GetHotelSlimDto>();

        // Country → Country DTOs
        CreateMap<Country, GetCountriesDto>();
        CreateMap<Country, GetCountryDto>();
        CreateMap<CreateCountryDto, Country>();
        CreateMap<UpdateCountryDto, Country>();
    }

    public class CountryNameResolver : IValueResolver<Hotel, GetHotelDto, string>
    {

        public string Resolve(Hotel source, GetHotelDto destination, string destMember, ResolutionContext context)
        {
            var countryName = source.Country?.Name ?? string.Empty;

            return countryName;

        }
    }
}
