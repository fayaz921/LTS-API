using LTS.API.Domain.Entities;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Mappings;

public static class CourtMappings
{
    public static GetAllCourtsDto ToDto(this Court court)
    {
        return new GetAllCourtsDto
        {
            Id = court.Id,
            CourtName = court.CourtName,
            AddressContact = court.AddressContact,
            IsActive = court.IsActive
        };
    }
}