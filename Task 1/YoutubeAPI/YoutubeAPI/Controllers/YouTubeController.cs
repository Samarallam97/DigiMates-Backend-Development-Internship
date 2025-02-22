namespace YoutubeAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class YouTubeController : ControllerBase
{
	private readonly IYouTubeService _youTubeService;

	public YouTubeController(IYouTubeService youTubeService)
	{
		_youTubeService = youTubeService;
	}

	[HttpGet]
	public async Task<IActionResult> GetYouTubeDetails( string id, string type)
	{
		var result = await _youTubeService.GetYouTubeDetailsAsync(id, type);

		if (result is null)
			return NotFound(new { error = "No data found from YouTube API." });

		return Ok(result);
	}
}
