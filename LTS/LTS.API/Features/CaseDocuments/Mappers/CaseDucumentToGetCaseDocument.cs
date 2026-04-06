using LTS.API.Domain.Entities;
using LTS.API.Features.CaseDocuments.DTOs;

namespace LTS.API.Features.CaseDocuments.Mappers
{
    public static class CaseDucumentToGetCaseDocument
    {
        public static GetCaseDocument Map(this CaseDocument document)
        {
            return new GetCaseDocument
            {
                Id = document.Id,
                CaseId = document.CaseId,
                FileName = document.FileName,
                FilePath = document.FilePath,
                FileType = document.FileType,
                FileSize = document.FileSize,
                Remarks = document.Remarks,
                CreatedAt = document.CreatedAt
            };
        }
        public static List<GetCaseDocument> MapList(this List<CaseDocument> documents)
        {
            return documents.Select(x =>x.Map()).ToList();
        }
    }
}
