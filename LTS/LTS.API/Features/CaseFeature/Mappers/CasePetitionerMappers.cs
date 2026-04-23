using LTS.API.Domain.Entities;
using LTS.API.Features.CaseFeature.Commands.CreateCase;

namespace LTS.API.Features.CaseFeature.Mappers
{
    public static class CasePetitionerMappers
    {
        public static CasePetitioner MapToCasePatitioner(this CreateCaseCommand command, Guid CaseId)
        {
            return new CasePetitioner()
            {
                Id = Guid.NewGuid(),
                CaseId = CaseId,
                PetitionerId = command.PetitionerId
            };
        }
    }
}
