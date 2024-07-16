using aranha.Interfaces;
using Microsoft.Playwright;

namespace aranha.Services
{
    public class LauncherPlaywrightService : ILauncherPlaywrightService
    {
        public async Task<IPlaywright> InitializePlaywrightAsync()
        {
            return await Playwright.CreateAsync();
        }
        public async Task<IBrowser> InitializePlaywrightBrowserAsync(IPlaywright playwright)
        {
            BrowserTypeLaunchOptions browserOptions = new() { Headless = false };
            return await playwright.Chromium.LaunchAsync(browserOptions);
        }
    }

    
}
