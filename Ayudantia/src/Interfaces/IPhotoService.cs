using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CloudinaryDotNet.Actions;

namespace Ayudantia.Src.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<ImageUploadResult> AddPhotoFromPathAsync(string filePath);
    }
}