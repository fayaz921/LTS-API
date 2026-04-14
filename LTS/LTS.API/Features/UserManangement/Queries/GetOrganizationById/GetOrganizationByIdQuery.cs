using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQuery : IRequest<ApiResponse<OrganizationDto>>
    {
        public Guid OrganizationId { get; set; }
    }
}
