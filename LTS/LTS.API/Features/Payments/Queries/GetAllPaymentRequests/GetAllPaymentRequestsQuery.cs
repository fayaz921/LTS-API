using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.Payments.DTOs;
using MediatR;

namespace LTS.API.Features.Payments.Queries.GetAllPaymentRequests
{
    public record GetAllPaymentRequestsQuery(
         int PageNumber = 1,
         int PageSize = 10,
         string? Search = null,
         PaymentStatus? Status = null
     ) : IRequest<ApiResponse<PaginatedResponse<PaymentRequestDto>>>;
}
