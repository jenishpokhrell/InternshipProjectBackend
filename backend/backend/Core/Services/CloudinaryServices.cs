using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Core.Entities;
using System.IO;

namespace backend.Core.Services
{
    public class CloudinaryServices
    {
        private readonly Cloudinary _cloudinary;

        //Configuring cloudinary
        public CloudinaryServices(IOptions<CloudinarySettings> _config)
        {
            var account = new Account(
                _config.Value.CloudName,
                _config.Value.ApiKey,
                _config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var allowedTypes = new List<string>
            {
                "image/jpg",
                "image/png",
                "image/jpeg",
            };

            if (!allowedTypes.Contains(file.ContentType))
            {
                throw new Exception("Invalid image type.");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadResumeAsync(IFormFile file)
        {
            var allowedTypes = new List<string>
            {
                "application/pdf",
                "application/msword"
            };

            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (!allowedTypes.Contains(file.ContentType))
            {
                throw new Exception("Invalid resume file. Please upload PDF or WORD file.");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = "resumes/" + Guid.NewGuid().ToString(),
                //ResourceType = "raw"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public string GetPublicId(string url)
        {
            var uri = new Uri(url);
            string path = uri.AbsolutePath;
            return Path.GetFileName(path);
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) return false;

            var publicId = GetPublicId(fileUrl);

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "Ok";
        }   

    }
}
