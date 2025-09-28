using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Country;
using HotelListing.Api.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{
    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {

        var countries = await countriesService.GetAllAsync();
        return ToActionResult(countries);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(Guid id)
    {
        var country = await countriesService.GetDetailsAsync(id);
        return ToActionResult(country);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(Guid id, UpdateCountryDto updateDto)
    {

        var result = await countriesService.UpdateAsync(id, updateDto);
        return ToActionResult(result);
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createDto)
    {
        var result = await countriesService.CreateAsync(createDto);
        if (!result.IsSuccess) return MapErrorsToResponse(result.Errors);
        return CreatedAtAction(nameof(GetCountry), new { id = result.Value!.CountryId }, result.Value);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(Guid id)
    {
        var result = await countriesService.DeleteAsync(id);
        return ToActionResult(result);
    }
}