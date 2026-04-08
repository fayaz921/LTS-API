using LTS.API.Domain.Entities;
using LTS.API.Features.Alerts.DTOs;

namespace LTS.API.Features.Alerts.Mappers
{
    public static class GetUpComingHearingDtoMapper
    {
        public static GetUpComingHearingDto Map(this Followup result)
        {
            return new GetUpComingHearingDto
            {
                CaseId = result.CaseId,
                CaseNo = result.Case.CaseNo,
                Title = result.Case.Title,
                NextHearingDate = result.NextHearingDate!.Value,
                EmailList = result.Case.EmailList
            };
        }
    }
}
