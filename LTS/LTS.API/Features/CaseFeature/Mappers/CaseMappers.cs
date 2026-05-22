using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.CaseFeature.Commands.CreateCase;
using LTS.API.Features.CaseFeature.Commands.UpdateCase;
using LTS.API.Features.CaseFeature.Queries.GetCases;

namespace LTS.API.Features.CaseFeature.Mappers
{
    public static class CaseMappers
    {
        public static Case Map(this CreateCaseCommand cmd, string caseNo)
        {
            return new Case()
            {
                Id = Guid.NewGuid(),
                CaseNo = caseNo,
                CourtId = cmd.CourtId,
                DepartmentId = cmd.DepartmentId,
                DAG = cmd.DAG,
                Title = cmd.Title,
                Subject = cmd.Subject,
                Detail = cmd.Detail,
                DateInstitution = cmd.DateInstitution,
                Status = CaseStatus.Pending,
                EmailList = cmd.EmailList,
                OrganizationId = cmd.OrganizationId,
            };

        }

        public static List<GetCaseDto> ToGetAllCasesDto(this IEnumerable<Case> cases)
        {
            if (cases == null || !cases.Any())
                return new List<GetCaseDto>();
            return cases.Select(c => c.ToGetCaseDto()).ToList();
        }
        public static GetCaseDto ToGetCaseDto(this Case c)
        {
            return new GetCaseDto(
                Id: c.Id,
                CaseNo: c.CaseNo,
                Title: c.Title,
                Subject: c.Subject,
                DAG: c.DAG,
                Status: c.Status.ToString(),
                DateInstitution: c.DateInstitution,
                CourtName: c.Court?.CourtName ?? "",
                DepartmentName: c.Department?.DepartmentName ?? "",
                c.CasePetitioners.Select(cp => new PetitionerDetailDto(
                           cp.Petitioner.Id,
                           cp.Petitioner.Name,
                           cp.Petitioner.CNIC,
                           cp.Petitioner.Email,
                           cp.Petitioner.Phone
                       )).ToList()
            );
        }
        public static Case MapToUpdatedCase(this UpdateCaseCommand cmd, Case oldeCase)
        {
            oldeCase.DepartmentId = cmd.DepartmentId;
            oldeCase.CourtId = cmd.CourtId;
            oldeCase.DAG = cmd.DAG;
            oldeCase.Detail = cmd.Detail;
            oldeCase.Title = cmd.Title;
            oldeCase.Subject = cmd.Subject;
            oldeCase.EmailList = cmd.EmailList;
            oldeCase.DateInstitution = cmd.DateInstitution;

            return oldeCase;
        }
    }
}
