using aranha.Entities;
using aranha.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace aranha.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [SwaggerTag("Crawler")]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
       private ICrawlerService _crawlerService;

        public CrawlerController(ICrawlerService crawlerService)
        {
            _crawlerService = crawlerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Crawler crawler)
        {
            string content = await _crawlerService.NavigateToUrlAsync(crawler.Url);
            return Ok(new { Content = content });
        }
    }
}
