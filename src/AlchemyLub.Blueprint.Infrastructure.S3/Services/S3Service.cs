namespace AlchemyLub.Blueprint.Infrastructure.S3.Services;

/// <inheritdoc cref="IS3Service"/>
internal sealed class S3Service(IConfiguration configuration, IMinioClient minioClient) : IS3Service
{
    /// <inheritdoc />
    public async Task<string> Upload(IFormFile file, CancellationToken cancellationToken = default)
    {
        string key = $"{DateTime.UtcNow:s}.{file.FileName}";

        List<string> allowedExtensions = [".doc", ".pdf", ".rtf", ".docx"];

        string fileExtension = Path.GetExtension(file.FileName);

        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new FileLoadException($"Invalid file extension - {fileExtension}. Valid: doc, docx, pdf, rtf");
        }

        int maxFileSizeBytes = 25 * 1024 * 1024;

        if (file.Length > maxFileSizeBytes)
        {
            throw new FileLoadException("File size exceeds the maximum allowed size of 25 MB.");
        }

        await using Stream fileStream = file.OpenReadStream();

        using var md5 = MD5.Create();

        byte[] hash = await md5.ComputeHashAsync(fileStream, cancellationToken);

        string md5Hash = Convert.ToBase64String(hash);

        fileStream.Position = 0;

        PutObjectArgs putObject = new PutObjectArgs()
            .WithBucket(configuration["AWS:Bucket"])
            .WithStreamData(fileStream)
            .WithObject(key)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        _ = await minioClient.PutObjectAsync(putObject, cancellationToken);

        return key;
    }

    /// <inheritdoc />
    public async Task<string> GetObjectPresignedUrl(string key, CancellationToken cancellationToken = default)
    {
        PresignedGetObjectArgs getObject = new PresignedGetObjectArgs()
            .WithBucket(configuration["AWS:Bucket"])
            .WithExpiry(1800)
            .WithObject(key);

        return await minioClient.PresignedGetObjectAsync(getObject);
    }
}
