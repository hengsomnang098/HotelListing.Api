using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : BaseApiController
{

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var hotels = await hotelsService.GetAllAsync();
        return ToActionResult(hotels);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(Guid id)
    {

        var hotel = await hotelsService.GetAsync(id);
        return ToActionResult(hotel);
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(Guid id, UpdateHotelDto hotelDto)
    {
        if (hotelDto.Id != id)
        {
            return BadRequest();
        }
        var result = await hotelsService.UpdateAsync(id, hotelDto);

        return ToActionResult(result);
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var result = await hotelsService.CreateAsync(hotelDto);
        if (!result.IsSuccess) return MapErrorsToResponse(result.Errors);

        return CreatedAtAction(nameof(GetHotel), new { id = result.Value!.Id }, result.Value);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        var result = await hotelsService.DeleteAsync(id);
        return ToActionResult(result);
    }

}