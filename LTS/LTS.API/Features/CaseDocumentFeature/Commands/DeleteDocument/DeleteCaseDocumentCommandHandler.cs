using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.CaseDocumentFeature.Commands.DeleteDocument
{
    public class DeleteCaseDocumentCommandHandler : IRequestHandler<DeleteCaseDocumentCommand, string>
    {
        private readonly AppDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public DeleteCaseDocumentCommandHandler(AppDbContext context,ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }


        public async Task<string> Handle(DeleteCaseDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = context.CaseDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken).Result;
            if (document is null)
               return "Document Not Found";
            if (!string.IsNullOrEmpty(document.PublicId))
            {
                var cloudDeleted=await cloudinaryService.DeleteFileAsync(document.PublicId,document.FileType);
                if (!cloudDeleted)
                     return "Failed to delete file from cloud storage";
            }
            context.CaseDocuments.Remove(document);
            await context.SaveChangesAsync(cancellationToken);
            return "Deleted Successfully";
        }
    }
}
