namespace LTS.API.Common.Response
{
    // ─────────────────────────────────────────────────────────────
    // Paginated Response Wrapper
    // Reusable across all modules (Courts, Departments, Cases, etc.)
    // ─────────────────────────────────────────────────────────────

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public static PaginatedResponse<T> Create(
            List<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            return new PaginatedResponse<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}