using Org.BouncyCastle.Bcpg.OpenPgp;

namespace LTS.API.Domain.Entities
{
    public class CaseDocument : BaseEntity
    {
        public Guid CaseId { get; set; }
        public string PublicId {  get; set; }= string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? Remarks { get; set; }

        // Navigation
        public Case Case { get; set; } = null!;
    }
}
