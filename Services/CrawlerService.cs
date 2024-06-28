using aranha.Interfaces;
using Microsoft.Playwright;

namespace aranha.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly IPlaywright _playwright;
        private readonly IBrowser _browser;

        public CrawlerService()
        {
            var task = Playwright.CreateAsync();

            task.Wait();

            _playwright = task.Result;

            var browserTask = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            browserTask.Wait();
            _browser = browserTask.Result;
        }
        public async Task<string> NavigateToUrlAsync(string url)
        {
            IPage page = await _browser.NewPageAsync();
            await page.GotoAsync(url);
            return await page.ContentAsync();
        }
    }
}
