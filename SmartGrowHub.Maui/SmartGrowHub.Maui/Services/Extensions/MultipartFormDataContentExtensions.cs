using System.Net.Http.Headers;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class MultipartFormDataContentExtensions
{
    public static MultipartFormDataContent AddStringContent(this MultipartFormDataContent formDataContent,
        string value, string name)
    {
        formDataContent.Add(new StringContent(value), name);
        return formDataContent;
    }

    public static MultipartFormDataContent AddImageContent(this MultipartFormDataContent formDataContent,
        Stream imageStream, string name, string fileName)
    {
        var frontFileContent = new StreamContent(imageStream);
        frontFileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        formDataContent.Add(frontFileContent, name, fileName);
        return formDataContent;
    }
}