using System.Text.Json.Serialization;

public sealed class CreateUploadResponse
{
    [JsonPropertyName("fileId")]
    public long FileId { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("upload_uri")]
    public string UploadUri { get; set; } = string.Empty;
}