using Edulytics.Web.YouTubeLearning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize]
[Route("api/lesson-youtube")]
public sealed class YouTubeLearningController(
    IYouTubeLearningService service) : ControllerBase
{
    [HttpGet("search")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Search(
        [FromQuery] string lessonCode,
        [FromQuery] string title,
        [FromQuery] string culture = "en",
        [FromQuery] string? q = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(lessonCode) ||
            string.IsNullOrWhiteSpace(title) ||
            title.Length > 240 ||
            lessonCode.Length > 240 ||
            (q?.Length ?? 0) > 180)
        {
            return BadRequest();
        }

        var result = await service.SearchAsync(
            lessonCode.Trim(),
            title.Trim(),
            culture,
            q,
            cancellationToken);

        return Ok(result);
    }
}
