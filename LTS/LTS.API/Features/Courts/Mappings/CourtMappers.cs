using LTS.API.Domain.Entities;
using LTS.API.Features.Courts.DTOs;

namespace LTS.API.Features.Courts.Mappings
{
    public static class CourtMappers
    {
        public static CourtDto ToDto(this Court court)
        {
            return new CourtDto
            {
                Id = court.Id,
                CourtName = court.CourtName,
                AddressContact = court.AddressContact,
                IsActive = court.IsActive,
                CreatedAt = court.CreatedAt
            };
        }
    }
}