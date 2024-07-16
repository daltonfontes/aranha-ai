using aranha.Interfaces;
using Microsoft.Playwright;

namespace aranha.Services
{
    public class CrawlerService : ICrawlerService
    {
        private readonly ILauncherPlaywrightService _launcherPlaywrightService;
        private IBrowser _browser;
        private IPlaywright _playwright;
        private byte[] _screenshot;

        public CrawlerService(ILauncherPlaywrightService launcherPlaywrightService)
        {
            _launcherPlaywrightService = launcherPlaywrightService;
            InitializeAsync().Wait();
        }

        public async Task<string> NavigateToUrlAsync(string url)
        {
            try
            {
                IPage page = await _browser.NewPageAsync();

                IBrowserContext context = await _browser.NewContextAsync(new BrowserNewContextOptions
                {
                    UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/109.0\r\n",
                    BypassCSP = true,
                    Locale = "de-DE",
                    ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
                });

                await page.SetExtraHTTPHeadersAsync(new Dictionary<string, string>
                {
                    { "Accept-Language", "en-US,en;q=0.9" },
                    { "Upgrade-Insecure-Requests", "1" }
                });

                await page.GotoAsync(url);

                _screenshot = await page.ScreenshotAsync();

                await SaveScreenshotToFileAsync("screenshot.png");

                return await page.ContentAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to navigate and capture screenshot: {ex.Message}", ex);
            }
        }

        public Task<byte[]> GetScreenshotAsync()
        {
            return Task.FromResult(_screenshot);
        }

        private async Task SaveScreenshotToFileAsync(string fileName)
        {
            if (_screenshot != null && _screenshot.Length > 0)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, _screenshot);
                Console.WriteLine($"Screenshot : {filePath}");
            }
            else
            {
                throw new InvalidOperationException("Screenshot saved.");
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                _playwright = await _launcherPlaywrightService.InitializePlaywrightAsync();
                _browser = await _launcherPlaywrightService.InitializePlaywrightBrowserAsync(_playwright);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initialize Playwright and launch browser: {ex.Message}", ex);
            }
        }

    }
}
