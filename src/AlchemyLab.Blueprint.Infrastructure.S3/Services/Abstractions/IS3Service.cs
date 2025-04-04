namespace AlchemyLab.Blueprint.Infrastructure.S3.Services.Abstractions;

/// <summary>
/// Сервис для работы с S3 хранилищем
/// </summary>
public interface IS3Service
{
    /// <summary>
    /// Загрузка файла
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Ключ файла</returns>
    Task<string> Upload(IFormFile file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить URL-адрес файла
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> GetObjectPresignedUrl(string key, CancellationToken cancellationToken = default);
}
