using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;

namespace LTS.API.Features.CaseDocumentFeature.Commands.UploadDocument
{
    public class UploadCaseDocumentCommandHandler:IRequestHandler<UploadCaseDocumentCommand, string>
    {
        private readonly AppDbContext context;

        public UploadCaseDocumentCommandHandler(AppDbContext Context,ICloudinaryService cloudinaryService)
        {
            context = Context;
            CloudinaryService = cloudinaryService;
        }

        public ICloudinaryService CloudinaryService { get; }

        public async Task<string> Handle(UploadCaseDocumentCommand request, CancellationToken cancellationToken)
        {
            var uploadResult = CloudinaryService.UploadFileAsync(request.File);
            var caseDocument = new Domain.Entities.CaseDocument
            {
                Id = Guid.NewGuid(),
                CaseId = request.caseId,
                FileName = request.FileName ?? request.File.FileName,
                FilePath = uploadResult.Result.Url,
                FileType= request.File.ContentType,
                FileSize=request.File.Length,
                Remarks = request.Remarks,
                PublicId= uploadResult.Result.PublicId,
                CreatedAt = DateTime.UtcNow
            };
            context.CaseDocuments.Add(caseDocument);
            await context.SaveChangesAsync(cancellationToken);
            return "Success";
        }
    }
}
