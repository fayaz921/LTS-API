using LTS.API.Common.Response;
using LTS.API.Features.CaseDocuments.Mappers;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;

namespace LTS.API.Features.CaseDocuments.Commands.UploadDocument
{
    public class UploadCaseDocumentCommandHandler : IRequestHandler<UploadCaseDocumentCommand, ApiResponse<string>>
    {
        private readonly AppDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public UploadCaseDocumentCommandHandler(AppDbContext Context, ICloudinaryService cloudinaryService)
        {
            context = Context;
            this.cloudinaryService = cloudinaryService;
        }


        public async Task<ApiResponse<string>> Handle(UploadCaseDocumentCommand request, CancellationToken cancellationToken)
        {
            var uploadResult = await cloudinaryService.UploadFileAsync(request.File);
            if (!uploadResult.IsSuccess)
                return ApiResponse<string>.Fail(uploadResult.Message,System.Net.HttpStatusCode.Conflict);
            var caseDocument = request.Map(uploadResult);
            context.CaseDocuments.Add(caseDocument);
            await context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!, "File Uploaded SuccessFully");
        }
    }
}
