using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogLab.Service
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);

        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}