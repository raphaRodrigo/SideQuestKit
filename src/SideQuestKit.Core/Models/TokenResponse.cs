namespace SideQuestKit.Core.Models;

public sealed class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime AccessTokenExpiresAt { get; set; }

    public string ClientId { get; set; } = string.Empty;

    public string UsersId { get; set; } = string.Empty;
}