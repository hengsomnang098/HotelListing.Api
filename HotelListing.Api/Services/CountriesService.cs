using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Api.Constants;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Country;
using HotelListing.Api.Results;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services
{


    public class CountriesService(HotelListingDbContext context, IMapper mapper) : ICountriesService
    {
        public async Task<Result<IEnumerable<GetCountriesDto>>> GetAllAsync()
        {
            var countries = await context.Countries
                .ProjectTo<GetCountriesDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<IEnumerable<GetCountriesDto>>.Success(countries);

        }

        public async Task<Result<GetCountryDto>> GetDetailsAsync(Guid id)
        {
            try
            {
                var country = await context.Countries
                    .Where(q => q.CountryId == id)
                    .ProjectTo<GetCountryDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();


                return country is null
                ? Result<GetCountryDto>.Failure(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."))
                : Result<GetCountryDto>.Success(country);
            }
            catch
            {
                return Result<GetCountryDto>.Failure(new Error(ErrorCodes.Failure, "An unexpected error occurred while retrieving the country details."));
            }
        }

        public async Task<Result<GetCountryDto>> CreateAsync(CreateCountryDto createDto)
        {
            try
            {
                var exists = await CountryExistsAsync(createDto.Name);
                if (exists)
                {
                    return Result<GetCountryDto>.Failure(new Error(ErrorCodes.Conflict, $"Country with name '{createDto.Name}' already exists."));
                }

                var country = mapper.Map<Country>(createDto);
                context.Countries.Add(country);
                await context.SaveChangesAsync();

                var dto = await context.Countries
                    .Where(c => c.CountryId == country.CountryId)
                    .ProjectTo<GetCountryDto>(mapper.ConfigurationProvider)
                    .FirstAsync();

                return Result<GetCountryDto>.Success(dto);
            }
            catch
            {
                return Result<GetCountryDto>.Failure(new Error(ErrorCodes.Failure, "An unexpected error occurred while creating the country."));
            }
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateCountryDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return Result.BadRequest(new Error(ErrorCodes.Validation, "Id route value does not match payload Id."));
                }

                var country = await context.Countries.FindAsync(id);
                if (country is null)
                {
                    return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
                }

                // Use AutoMapper to map incoming DTO onto the tracked entity
                mapper.Map(updateDto, country);
                await context.SaveChangesAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure(new Error(ErrorCodes.Failure, "An unexpected error occurred while updating the country."));
            }
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country is null)
            {
                return Result.NotFound(new Error(ErrorCodes.NotFound, $"Country '{id}' was not found."));
            }

            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<bool> CountryExistsAsync(Guid id)
        {
            return await context.Countries.AnyAsync(e => e.CountryId == id);
        }

        public async Task<bool> CountryExistsAsync(string name)
        {
            return await context.Countries
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
    }

}