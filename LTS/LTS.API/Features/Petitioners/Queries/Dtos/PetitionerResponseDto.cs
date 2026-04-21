namespace LTS.API.Features.Petitioners.Queries.Dtos
{
    public class PetitionerResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Business { get; set; }
        public string? Email { get; set; }
        public string? CNIC { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
