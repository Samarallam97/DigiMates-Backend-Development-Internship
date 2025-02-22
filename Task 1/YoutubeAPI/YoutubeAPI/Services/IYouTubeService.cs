namespace YoutubeAPI.Services;

public interface IYouTubeService
{
	Task<YouTubeResponseDto?> GetYouTubeDetailsAsync(string id, string type);
}
