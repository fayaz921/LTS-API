namespace LTS.API.Common.Response
{
    public class CasesPaginatedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int PendingCount { get; set; }
        public int FinalizedCount { get; set; }
    }
}
