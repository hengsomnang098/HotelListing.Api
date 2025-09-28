using HotelListing.Api.DTOs.Hotel;

namespace HotelListing.Api.DTOs.Country;

public record GetCountryDto
(
    Guid CountryId,
    string Name,
    string ShortName,
    List<GetHotelSlimDto> Hotels
)
{
}


public record GetCountriesDto
(
    Guid CountryId,
    string Name,
    string ShortName
);
