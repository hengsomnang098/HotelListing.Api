using HotelListing.Api.DTOs.Country;
using HotelListing.Api.Results;

namespace HotelListing.Api.Services
{
    public interface ICountriesService
    {
        Task<bool> CountryExistsAsync(Guid id);
        Task<bool> CountryExistsAsync(string name);
        Task<Result<IEnumerable<GetCountriesDto>>> GetAllAsync();
        Task<Result<GetCountryDto>> GetDetailsAsync(Guid id);
        Task<Result<GetCountryDto>> CreateAsync(CreateCountryDto createDto);
        Task<Result> UpdateAsync(Guid id, UpdateCountryDto updateDto);

        Task<Result> DeleteAsync(Guid id);


    }
}