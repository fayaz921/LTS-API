namespace LTS.API.Domain.Entities
{
    public class CaseNumberSequence
    {
        public int Id { get; set; }
        public Guid OrganizationId { get; set; } 
        public int Year { get; set; }
        public int LastSequence { get; set; }
    }
}
