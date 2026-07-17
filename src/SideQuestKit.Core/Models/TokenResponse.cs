using System.Text.Json.Serialization;

namespace SideQuestKit.Core.Models;

public sealed class TokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("accessTokenExpiresAt")]
    public DateTime AccessTokenExpiresAt { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("users_id")]
    public string UsersId { get; set; } = string.Empty;
}