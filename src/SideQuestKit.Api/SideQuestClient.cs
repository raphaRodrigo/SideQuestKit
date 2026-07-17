using System.Net.Http.Json;
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

    public async Task<TokenResponse>
        RefreshTokenAsync(
            string refreshToken)
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
}