using HotelListing.Api.DTOs.Country;

namespace HotelListing.Api.DTOs.Hotel
{
    public record GetHotelDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public double Rating { get; init; }
        public Guid CountryId { get; init; }
        public string Country { get; init; }
    }


    public record GetHotelSlimDto(
        Guid Id,
        string Name,
        string Address,
        double Rating
    );


}