using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Api.Constants;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Hotel;
using HotelListing.Api.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services
{
    public class HotelsService(HotelListingDbContext context, ICountriesService countriesService, IMapper mapper) : IHotelsService
    {
        public async Task<Result<IEnumerable<GetHotelDto>>> GetAllAsync()
        {
            var hotels = await context.Hotels
                .Include(q => q.Country) // optional, EF sometimes fetches it anyway
                .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Result<IEnumerable<GetHotelDto>>.Success(hotels);
        }

        public async Task<Result<GetHotelDto>> GetAsync(Guid id)
        {
            var hotel = await context.Hotels
                .Include(h => h.Country) // Eager loading the Country navigation property
                .Where(q => q.Id == id)
                .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return hotel is not null
                ? Result<GetHotelDto>.Success(hotel)
                : Result<GetHotelDto>.NotFound();

        }

        public async Task<Result<GetHotelDto>> CreateAsync(CreateHotelDto createDto)
        {
            try
            {
                var hotel = mapper.Map<Hotel>(createDto);
                context.Hotels.Add(hotel);
                await context.SaveChangesAsync();
                var result = mapper.Map<GetHotelDto>(hotel);
                return Result<GetHotelDto>.Success(result);
            }
            catch
            {
                return Result<GetHotelDto>.Failure(new Error(ErrorCodes.Failure, "An unexpected error occurred while creating the hotel."));

            }


        }

        public async Task<Result> UpdateAsync(Guid id, UpdateHotelDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return Result.Failure(new Error(ErrorCodes.Validation, "Mismatched Hotel ID"));
            }
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel is null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Hotel '{id}' was not found."));
            }
            var countryExists = await countriesService.CountryExistsAsync(updateDto.CountryId);
            if (!countryExists)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{updateDto.CountryId}' was not found."));
            }

            mapper.Map(updateDto, hotel);

            context.Hotels.Update(hotel);
            await context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var affected = await context.Hotels
                .Where(q => q.Id == id)
                .ExecuteDeleteAsync();

            if (affected == 0)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Hotel '{id}' was not found."));
            }

            return Result.Success();
        }

        public Task<bool> HotelExistsAsync(Guid id)
        {
            return context.Hotels.AnyAsync(e => e.Id == id);
        }

        public Task<bool> HotelExistsAsync(string name, Guid countryId)
        {
            return context.Hotels.AnyAsync(h => h.Name == name && h.CountryId == countryId);
        }
    }
}