using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseDocuments.Commands.DeleteDocument
{
    public class DeleteCaseDocumentCommandHandler : IRequestHandler<DeleteCaseDocumentCommand, ApiResponse<string>>
    {
        private readonly AppDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public DeleteCaseDocumentCommandHandler(AppDbContext context,ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }


        public async Task<ApiResponse<string>> Handle(DeleteCaseDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = context.CaseDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).Result;
            if (document is null)
               return ApiResponse<string>.Fail($"Document with Id {request.Id} not found.", Domain.Enums.ResponseType.NotFound);
            if (!string.IsNullOrEmpty(document.PublicId))
            {
                var cloudDeleted=await cloudinaryService.DeleteFileAsync(document.PublicId,document.FileType);
                if (!cloudDeleted)
                     return ApiResponse<string>.Fail("Failed to delete the document from cloud storage.", Domain.Enums.ResponseType.ServerError);
            }
            context.CaseDocuments.Remove(document);
            await context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok("Document deleted successfully.");
        }
    }
}
