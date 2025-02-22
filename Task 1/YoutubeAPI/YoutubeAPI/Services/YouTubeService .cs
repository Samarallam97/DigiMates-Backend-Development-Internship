namespace YoutubeAPI.Services;

public class YouTubeService  : IYouTubeService
{
	private readonly HttpClient _httpClient;
	private readonly string _apiKey;

	public YouTubeService(HttpClient httpClient, IConfiguration configuration)
	{
		_httpClient = httpClient;
		_apiKey = configuration["YouTubeApi:ApiKey"] ?? throw new InvalidOperationException("API key is missing.");
	}

	public async Task<YouTubeResponseDto?> GetYouTubeDetailsAsync(string id, string type)
	{
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(type))
			return null;

		string? url = type switch
		{
			"video" or "short" => $"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key={_apiKey}",
			"playlist" => $"https://www.googleapis.com/youtube/v3/playlists?part=snippet&id={id}&key={_apiKey}",
			_ => null
		};

		if (url is null)
			return null;

		var response = await _httpClient.GetStringAsync(url);

		return ParseYouTubeResponse(response);
	}

	private YouTubeResponseDto? ParseYouTubeResponse(string jsonResponse)
	{
		var document = JsonDocument.Parse(jsonResponse);

		if (!document.RootElement.TryGetProperty("items", out var items) || items.GetArrayLength() == 0)
			return null;

		var snippet = items[0].GetProperty("snippet");

		return new YouTubeResponseDto
		{
			Title = snippet.GetProperty("title").GetString() ?? "Unknown Title",
			Description = snippet.GetProperty("description").GetString() ?? "No Description",
			PublishedDate = snippet.GetProperty("publishedAt").GetString() ?? "Unknown Date",
			PictureUrl = snippet.GetProperty("thumbnails").GetProperty("high").GetProperty("url").GetString() ?? "No Image",
			ChannelTitle = snippet.GetProperty("channelTitle").GetString() ?? "Unknown Channel"
		};
	}
}
