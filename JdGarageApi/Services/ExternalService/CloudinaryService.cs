using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public Task<string> UploadBrandImageAsync(IFormFile file) => UploadImageAsync(file, folder: "jdgarage/brands", allowedExtensions: new[] { ".png" }, allowedContentTypes: new[] { "image/png" });

    public Task<string> UploadBikeImageAsync(IFormFile file) => UploadImageAsync(file, folder: "jdgarage/bikes", allowedExtensions: null, allowedContentTypes: new[] { "image/" });

    public Task<string> UploadCarImageAsync(IFormFile file) => UploadImageAsync(file, folder: "jdgarage/cars", allowedExtensions: null, allowedContentTypes: new[] { "image/" });

    private async Task<string> UploadImageAsync(IFormFile file, string folder, string[]? allowedExtensions, string[]? allowedContentTypes)
    {
        ValidateImage(file, allowedExtensions, allowedContentTypes);
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            UseFilename = false,
            UniqueFilename = true
        };
        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Error subiendo la imagen");
        return result.SecureUrl.ToString();
    }

    private static void ValidateImage(IFormFile file, string[]? allowedExtensions, string[]? allowedContentTypes)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Imagen inválida");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (allowedExtensions != null && !allowedExtensions.Contains(extension))
            throw new Exception("Formato de imagen no permitido");

        if (allowedContentTypes != null)
        {
            var isValidContentType = allowedContentTypes.Any(ct =>
                ct.EndsWith("/")
                    ? file.ContentType.StartsWith(ct)
                    : file.ContentType.Equals(ct, StringComparison.OrdinalIgnoreCase)
            );

            if (!isValidContentType)
                throw new Exception("Tipo de imagen no permitido");
        }
    }
}
