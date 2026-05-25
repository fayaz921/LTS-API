using LTS.API.Common.Response;
using LTS.API.Features.Payments.DTOs;
using LTS.API.Features.Payments.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Payments.Queries.GetAllPaymentRequests
{
    public sealed class GetAllPaymentRequestsQueryHandler
         : IRequestHandler<GetAllPaymentRequestsQuery, ApiResponse<PaginatedResponse<PaymentRequestDto>>>
    {
        private readonly AppDbContext _context;

        public GetAllPaymentRequestsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedResponse<PaymentRequestDto>>> Handle(
            GetAllPaymentRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.PaymentRequests
                .AsNoTracking()
                .Include(p => p.Organization)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(p =>
                    p.SenderName.ToLower().Contains(search) ||
                    p.TransactionId.ToLower().Contains(search) ||
                    p.Organization.OrganizationName.ToLower().Contains(search));
            }

            if (request.Status.HasValue)
                query = query.Where(p => p.Status == request.Status.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.SubmittedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => p.ToDto())
                .ToListAsync(cancellationToken);

            if (items.Count == 0)
                return ApiResponse<PaginatedResponse<PaymentRequestDto>>.Ok(
                    PaginatedResponse<PaymentRequestDto>.Create([], totalCount, request.PageNumber, request.PageSize),
                    "No payment requests found");

            var paginated = PaginatedResponse<PaymentRequestDto>.Create(
                items, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PaginatedResponse<PaymentRequestDto>>.Ok(
                paginated, "Payment requests fetched successfully");
        }
    }
}
