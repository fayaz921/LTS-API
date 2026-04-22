using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.CaseFeature.Commands.CreateCase
{
    public class CreateCaseHandler(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<ApiResponse<string>> Handle(CreateCaseCommand request, CancellationToken ct)
        {
            var newCaseNo = await GenerateCaseNo(request.OrganizationId);
            await _context.Cases.AddAsync(request.Map(newCaseNo), ct);
            return await _context.SaveChangesAsync(ct) > 0 ? ApiResponse<string>.Created(default!) :
                                                           ApiResponse<string>.Fail("Internel Server Error!", HttpStatusCode.InternalServerError);
        }

        public async Task<string> GenerateCaseNo(Guid organizationId)
        {
            var currentYear = DateTime.UtcNow.Year;
            var sequence = await _context.CaseNumberSequences.Where(x => x.Year == currentYear && x.OrganizationId == organizationId).FirstOrDefaultAsync();
            if (sequence == null)
            {
                sequence = new CaseNumberSequence
                {
                    OrganizationId = organizationId,
                    Year = currentYear,
                    LastSequence = 1
                };
                await _context.CaseNumberSequences.AddAsync(sequence);
            }
            else
            {
                int lastCaseNo = sequence.LastSequence++;
                await _context.CaseNumberSequences.ExecuteUpdateAsync(x => x.SetProperty(x => x.LastSequence, lastCaseNo));
            }
            return $"LTS-{currentYear}-{sequence.LastSequence:D4}";
        }
    }
}
