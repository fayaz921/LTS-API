namespace LTS.API.Features.CaseDocuments.DTOs
{
    public class GetCaseDocument
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
