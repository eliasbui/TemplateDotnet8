using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Serilog.Events;
using Template.Contract.Extensions;

namespace Template.Contract.Service.Version.Contractors;

public class RestClientService(RestClient? client, ILogger<RestClientService> logger) : IRestClientService
{
    private const string ClassName = nameof(RestClientService);

    public async Task<T> PostAsync<T>(string endpointUrl, object body)
    {
        try
        {
            client = new RestClient();
            var request = new RestRequest(endpointUrl, Method.Post);
            var jsonRequest = JsonConvert.SerializeObject(body);
            request.AddJsonBody(jsonRequest, contentType: ContentType.Json);

            var response = await client.ExecuteAsync(request);

            if (response == null)
            {
                logger.LogError(
                    $"RestClientService make POST request to endpoint {endpointUrl} returned null".GeneratedLog(
                        ClassName, LogEventLevel.Error));
                throw new ApplicationException(
                    $"RestClientService make POST request to endpoint {endpointUrl} returned null",
                    new Exception("Error"));
            }

            if (response.ErrorException != null)
            {
                logger.LogError(
                    $"Got exception when execute request to {endpointUrl} | Exception: {response.ErrorMessage} | Content: {response.Content}"
                        .GeneratedLog(ClassName, LogEventLevel.Fatal));
                throw new ApplicationException("Error executing POST request", response.ErrorException);
            }

            if (string.IsNullOrEmpty(response.Content))
            {
                logger.LogError(
                    $"Execute POST request to endpoint {endpointUrl} not throw exception but does not contain content"
                        .GeneratedLog(ClassName, LogEventLevel.Error));
                throw new Exception("Not contain content");
            }

            var result = JsonConvert.DeserializeObject<T>(response.Content);
            if (result != null)
            {
                return result;
            }

            logger.LogError(
                $"Execute POST request to endpoint {endpointUrl} || Mapping content is null || Content: {response.Content} || T model type : {nameof(T)}"
                    .GeneratedLog(ClassName, LogEventLevel.Error));
            throw new Exception("Content is null or empty");
        }
        catch (Exception e)
        {
            logger.LogError(
                $@"Got exception when execute request to {endpointUrl} | Exception: {e.Message}".GeneratedLog(ClassName,
                    LogEventLevel.Fatal));
            throw new Exception("Error executing POST request", e);
        }
        finally
        {
            logger.LogInformation("End {ClassName} {MethodName}", ClassName, "PostAsync");
        }
    }

    public async Task<FileResult> GetFileAsync(string endpointUrl, object body, string defaultFileName)
    {
        try
        {
            var restClient = new RestClient();
            var request = new RestRequest(endpointUrl, Method.Post);
            var jsonRequest = JsonConvert.SerializeObject(body);
            request.AddJsonBody(jsonRequest, contentType: ContentType.Json);
            logger.LogInformation(
                $"Sending request to: {endpointUrl}".GeneratedLog(ClassName, LogEventLevel.Information));

            // Execute the request asynchronously
            var response = await restClient.ExecuteAsync(request);

            // Check if the response has content and status is OK
            if (response is { StatusCode: HttpStatusCode.OK, RawBytes: not null })
            {
                // Extract filename from the Content-Disposition header
                var contentDispositionHeader = response.Headers?.FirstOrDefault(h =>
                    h.Name != null && h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase));
                string fileName = defaultFileName; // Default filename
                if (contentDispositionHeader != null)
                {
                    var contentDisposition =
                        new System.Net.Mime.ContentDisposition(contentDispositionHeader.Value?.ToString() ??
                                                               string.Empty);
                    fileName = contentDisposition.FileName ?? defaultFileName;
                }

                logger.LogInformation($"Successfully retrieved file: {fileName}");

                var memoryStream = new MemoryStream(response.RawBytes);
                return new FileStreamResult(memoryStream, "application/octet-stream") { FileDownloadName = fileName };
            }
            else
            {
                logger.LogError(
                    $"Failed to retrieve file. Status: {response.StatusCode}. Content: {response.Content}".GeneratedLog(
                        ClassName, LogEventLevel.Error));
                throw new Exception(
                    $"Failed to retrieve file. Status: {response.StatusCode}. Content: {response.Content}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching file from service.");
            throw;
        }
    }

    public async Task<T?> PostAsync<T>(string serviceUrl, string apiPath, object body)
    {
        try
        {
            client = new RestClient(serviceUrl);
            var request = new RestRequest(apiPath, Method.Post);
            request.AddJsonBody(JsonConvert.SerializeObject(body), contentType: ContentType.Json);

            var response = await client.ExecuteAsync(request);
            if (response.ErrorException == null && response.Content != null)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            logger.LogError(
                $"Got exception when execute request to {Path.Combine(serviceUrl, apiPath)} | Exception: {response.ErrorException?.Message}"
                    .GeneratedLog(ClassName, LogEventLevel.Fatal));
            throw new ApplicationException("Error executing POST request", response.ErrorException);
        }
        catch (Exception e)
        {
            logger.LogError(
                $"Got exception when execute request to {Path.Combine(serviceUrl, apiPath)} | Exception: {e.Message}"
                    .GeneratedLog(ClassName, LogEventLevel.Fatal));
            throw new Exception("Error executing POST request", e);
        }
    }

    public async Task<T?> PostSendFile<T>(string serviceUrl, string apiPath, IFormFile file, string fileName)
    {
        try
        {
            client = new RestClient(serviceUrl);
            var request = new RestRequest(apiPath, Method.Post);
            await using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);
            var fileByte = memoryStream.ToArray();

            //Add file to request
            request.AddFile("file", fileByte, fileName);

            var response = await client.ExecuteAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.BadRequest) ||
                (response.ErrorException == null && response.Content != null))
            {
                if (response.Content != null)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }

            logger.LogError(
                $"Got exception when execute request to {Path.Combine(serviceUrl, apiPath)} | Exception: {response.ErrorException?.Message}"
                    .GeneratedLog(ClassName, LogEventLevel.Fatal));
            throw new ApplicationException("Error executing POST request", response.ErrorException);
        }
        catch (Exception e)
        {
            logger.LogError(
                $"Got exception when execute request to {Path.Combine(serviceUrl, apiPath)} | Exception: {e.Message}"
                    .GeneratedLog(ClassName, LogEventLevel.Fatal));
            throw new Exception("Error executing POST request", e);
        }
    }

    public async Task<FileStreamResult> GetFileStreamResultAsync(string endpointUrl, object body,
        string defaultFileName)
    {
        try
        {
            var restClient = new RestClient();
            var request = new RestRequest(endpointUrl, Method.Post);
            var jsonRequest = JsonConvert.SerializeObject(body);
            request.AddJsonBody(jsonRequest, contentType: ContentType.Json);
            logger.LogInformation(
                $"Sending request to: {endpointUrl}".GeneratedLog(ClassName, LogEventLevel.Information));

            // Execute the request asynchronously
            var response = await restClient.ExecuteAsync(request);

            // Check if the response has content and status is OK
            if (response is { StatusCode: HttpStatusCode.OK, RawBytes: not null })
            {
                // Extract filename from the Content-Disposition header
                var contentDispositionHeader = response.Headers?.FirstOrDefault(h =>
                    h.Name != null && h.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase));
                string fileName = defaultFileName; // Default filename
                if (contentDispositionHeader != null)
                {
                    var contentDisposition =
                        new System.Net.Mime.ContentDisposition(contentDispositionHeader.Value?.ToString() ??
                                                               string.Empty);
                    fileName = contentDisposition.FileName ?? defaultFileName;
                }

                logger.LogInformation($"Successfully retrieved file: {fileName}");

                var memoryStream = new MemoryStream(response.RawBytes!);
                return new FileStreamResult(memoryStream, "application/octet-stream") { FileDownloadName = fileName };
            }
            else
            {
                logger.LogError(
                    $"Failed to retrieve file. Status: {response.StatusCode}. Content: {response.Content}".GeneratedLog(
                        ClassName, LogEventLevel.Error));
                throw new Exception(
                    $"Failed to retrieve file. Status: {response.StatusCode}. Content: {response.Content}");
            }
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error occurred during HTTP request.");
            throw new Exception("Failed to retrieve file. Error during HTTP request.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            throw new Exception("Failed to retrieve file. Unexpected error.", ex);
        }
    }
}
