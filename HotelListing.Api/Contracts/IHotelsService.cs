using HotelListing.Api.DTOs.Hotel;
using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts
{
    public interface IHotelsService
    {
        Task<Result<IEnumerable<GetHotelDto>>> GetAllAsync();
        Task<Result<GetHotelDto>> GetAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, UpdateHotelDto updateDto);
        Task<Result<GetHotelDto>> CreateAsync(CreateHotelDto createDto);
        Task<Result> DeleteAsync(Guid id);
        Task<bool> HotelExistsAsync(Guid id);
        Task<bool> HotelExistsAsync(string name, Guid countryId);

    }
}