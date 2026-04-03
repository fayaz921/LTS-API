namespace LTS.API.Domain.Entities
{
    public class CasePetitioner
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public Guid PetitionerId { get; set; }

        // Navigation
        public Case Case { get; set; } = null!;
        public Petitioner Petitioner { get; set; } = null!;
    }
}
