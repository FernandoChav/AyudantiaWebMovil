using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Interfaces;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Ayudantia.Src.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IConfiguration config)
        {
            var acc = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            _ = new ImageUploadResult();

            if (file.Length == 0 || file.Length > 100 * 1024 * 1024) // 100MB
                throw new ArgumentException("Archivo no válido o excede el tamaño permitido (100MB)");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Formato de imagen no compatible");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "products"
            };

            ImageUploadResult? uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }
        public async Task<ImageUploadResult> AddPhotoFromPathAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"No se encontró el archivo: {filePath}");

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Formato de imagen no compatible");

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string publicId = $"products/{fileNameWithoutExt}";

            // Verificar si ya existe en Cloudinary
            try
            {
                var existing = await _cloudinary.GetResourceAsync(publicId);
                if (existing != null && !string.IsNullOrEmpty(existing.SecureUrl))
                {
                    return new ImageUploadResult
                    {
                        SecureUrl = new Uri(existing.SecureUrl),
                        PublicId = existing.PublicId
                    };
                }
            }
            catch (Exception ex)
            {
                // Puede ser que no exista, o que haya error de conexión
                if (!ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    throw new ApplicationException("Error al verificar la existencia de la imagen en Cloudinary", ex);
            }

            // Si no existe, subirla
            try
            {
                await using var stream = File.OpenRead(filePath);
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(Path.GetFileName(filePath), stream),
                    Folder = "products",
                    PublicId = fileNameWithoutExt, // Evita duplicación
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new ApplicationException($"Error al subir imagen '{filePath}': {uploadResult.Error.Message}");
                }

                return uploadResult;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error inesperado al subir imagen '{filePath}' a Cloudinary", ex);
            }
        }



        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "not found")
                return new DeletionResult { Result = "ok" };

            return result;
        }

    }
}