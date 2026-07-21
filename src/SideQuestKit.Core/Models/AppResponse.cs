using System.Text.Json.Serialization;

namespace SideQuestKit.Core.Models;

public sealed class AppResponse
{
    [JsonPropertyName("apps_id")]
    public long AppsId { get; set; }

    [JsonPropertyName("versionname")]
    public string Versionname { get; set; } = "";

    [JsonPropertyName("versioncode")]
    public int Versioncode { get; set; }

    [JsonPropertyName("apk_url")]
    public string ApkUrl { get; set; } = "";

    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public string Packagename { get; set; } = "";
}