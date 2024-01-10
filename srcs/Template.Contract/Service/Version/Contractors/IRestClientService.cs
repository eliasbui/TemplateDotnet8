using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Template.Contract.Service.Version.Contractors;

public interface IRestClientService
{
    Task<T> PostAsync<T>(string endpointUrl, object body);
    Task<FileResult> GetFileAsync(string endpointUrl, object body, string defaultFileName);
    Task<T?> PostAsync<T>(string serviceUrl, string apiPath, object body);
    Task<T?> PostSendFile<T>(string serviceUrl, string apiPath, IFormFile file, string fileName);
    Task<FileStreamResult> GetFileStreamResultAsync(string endpointUrl, object body, string defaultFileName);
}
