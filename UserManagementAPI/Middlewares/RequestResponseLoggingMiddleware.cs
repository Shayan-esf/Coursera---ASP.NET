using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log the request
        var request = await FormatRequest(context.Request);
        System.Console.WriteLine("Request: " + request);

        // Copy the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            // Continue to the next middleware
            await _next(context);

            // Log the response
            var response = await FormatResponse(context.Response);
            System.Console.WriteLine("Response: " + response);

            // Copy the contents of the new memory stream (which contains the response) to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;
        return $"Schema:{request.Scheme} Host: {request.Host} Path: {request.Path} QueryString: {request.QueryString} Request Body: {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return $"Response Body: {text}";
    }
}
