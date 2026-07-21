using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using SideQuestKit.Core.Models;

namespace SideQuestKit.Api;

public sealed class SideQuestClient
{
    private readonly HttpClient _httpClient;

    public SideQuestClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress =
                new Uri(
                    "https://api.sidequestvr.com/")
        };
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var content =
            new FormUrlEncodedContent(
            [
                new(
                    "client_id",
                    "sidequest"),

                new(
                    "grant_type",
                    "refresh_token"),

                new(
                    "refresh_token",
                    refreshToken)
            ]);

        var response =
            await _httpClient.PostAsync(
                "v2/oauth/token",
                content);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<TokenResponse>()
            ?? throw new Exception(
                "Failed to deserialize response.");
    }

    public async Task<CreateUploadResponse> CreateUploadAsync(string accessToken, FileInfo apk)
    {
        using var client =
            new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                accessToken);

        var payload = new
        {
            name = apk.Name,
            size = apk.Length,
            type = "apk"
        };

        var response =
            await client.PostAsJsonAsync(
                "https://cdn.sidequestvr.com/create-upload",
                payload);

        Console.WriteLine(
            $"Status Code: {(int)response.StatusCode} {response.StatusCode}");

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine(
            responseBody);

        response.EnsureSuccessStatusCode();

        return JsonSerializer
            .Deserialize<CreateUploadResponse>(
                responseBody)
            ?? throw new Exception(
                "Failed to deserialize response.");
    }

    public async Task UploadFileAsync(string uploadUri, string contentType, FileInfo apk)
    {
        await using var stream =
            apk.OpenRead();

        using var content =
            new StreamContent(stream);

        content.Headers.ContentType =
            new MediaTypeHeaderValue(
                contentType);

        var response =
            await _httpClient.PutAsync(
                uploadUri,
                content);

        Console.WriteLine(
            $"Status Code: {(int)response.StatusCode} {response.StatusCode}");

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine(
            responseBody);

        response.EnsureSuccessStatusCode();
    }

    public async Task AssociateApkAsync(string accessToken, string appId, CreateUploadResponse upload)
    {
        var fileUrl =
            $"https://cdn.sidequestvr.com/{upload.Path}";

        var payload =
            new
            {
                urls = new[]
                {
                    new
                    {
                        link_url =
                            JsonSerializer.Serialize(
                                new Dictionary<string, string>
                                {
                                    ["1"] = fileUrl
                                }),

                        provider = "APK"
                    }
                }
            };

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                accessToken);

        var response =
            await _httpClient.PutAsJsonAsync(
                $"v2/developers/apps/{appId}/urls",
                payload);

        Console.WriteLine(
            $"Status Code: {(int)response.StatusCode} {response.StatusCode}");

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine(
            responseBody);

        response.EnsureSuccessStatusCode();
    }
}