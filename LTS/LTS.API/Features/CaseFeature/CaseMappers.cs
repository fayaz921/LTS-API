using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.CaseFeature.Commands.CreateCase;
using LTS.API.Features.CaseFeature.Commands.UpdateCase;
using LTS.API.Features.CaseFeature.Queries.GetCases;

namespace LTS.API.Features.CaseFeature
{
    public static class CaseMappers
    {
        public static Case Map(this CreateCaseCommand cmd)
        {
            return new Case()
            {
                Id = Guid.NewGuid(),
                CaseNo = "1234",       // current a temperory number is given 
                CourtId = cmd.CourtId,
                DepartmentId = cmd.DepartmentId,
                DAG = cmd.DAG,
                Title = cmd.Title,
                Subject = cmd.Subject,
                Detail = cmd.Detail,
                DateInstitution = cmd.DateInstitution,
                Status = CaseStatus.Pending,
                EmailList = cmd.EmailList,
            };

        }

        public static List<GetCasesDto> ToDto(this IEnumerable<Case> cases)
        {
            return cases.Select(c => c.ToDto()).ToList();
        }
        public static GetCasesDto ToDto(this Case c)
        {
            return new GetCasesDto(
                Id: c.Id,
                CaseNo: c.CaseNo,
                Title: c.Title,
                Subject: c.Subject,
                DAG: c.DAG,
                Status: c.Status.ToString(),
                DateInstitution: c.DateInstitution,
                CourtName: c.Court?.CourtName ?? "",
                DepartmentName: c.Department?.DepartmentName ?? "",
                Petitioners: c.CasePetitioners
                                 .Select(cp => cp.Petitioner.Name)
                                 .ToList()
            );
        }
        public static Case MapToUpdatedCase(this UpdateCaseCommand cmd, Case oldeCase)
        {
            oldeCase.DepartmentId = cmd.DepartmentId;
            oldeCase.CourtId = cmd.CourtId;
            oldeCase.DAG=cmd.DAG;
            oldeCase.Detail= cmd.Detail;
            oldeCase.Title=cmd.Title;
            oldeCase.Subject=cmd.Subject;
            oldeCase.EmailList=cmd.EmailList;
            oldeCase.DateInstitution = cmd.DateInstitution;

            return oldeCase;
        }
    }
}
