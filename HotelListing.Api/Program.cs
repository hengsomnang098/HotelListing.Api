using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.MappingProfiles;
using HotelListing.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Required for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// conntion to database
try
{

    var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
    builder.Services.AddDbContext<HotelListingDbContext>(options =>
        options.UseNpgsql(connectionString));

    builder.Services.AddAutoMapper((cfg) =>
    {
        cfg.AddProfile<MappingProfile>();
    });

    builder.Services.AddScoped<ICountriesService, CountriesService>();
    // 
    builder.Services.AddScoped<IHotelsService, HotelsService>();
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); // Generates /swagger/v1/swagger.json
        app.UseSwaggerUI(); // Serves Swagger UI at /swagger
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    // Log database configuration error
    Console.WriteLine($"Database configuration error: {ex.Message}");
    throw;
}




