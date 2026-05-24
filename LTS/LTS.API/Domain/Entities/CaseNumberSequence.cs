using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTS.API.Domain.Entities
{
    public class CaseNumberSequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrganizationId { get; set; } 
        public int Year { get; set; }
        public int LastSequence { get; set; }
    }
}
