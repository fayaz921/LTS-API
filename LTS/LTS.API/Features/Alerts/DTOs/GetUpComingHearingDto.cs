namespace LTS.API.Features.Alerts.DTOs
{
    public class GetUpComingHearingDto
    {

        public Guid CaseId { get; set; }
        public string CaseNo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime NextHearingDate { get; set; }
        public string EmailList { get; set; } = string.Empty;
    }
}
