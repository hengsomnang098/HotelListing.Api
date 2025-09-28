

using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.DTOs.Hotel
{
    public class UpdateHotelDto : CreateHotelDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}