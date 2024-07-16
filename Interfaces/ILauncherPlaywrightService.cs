using Microsoft.Playwright;

namespace aranha.Interfaces
{
    public interface ILauncherPlaywrightService
    {
        Task<IPlaywright> InitializePlaywrightAsync();
        Task<IBrowser> InitializePlaywrightBrowserAsync(IPlaywright playwright);
    }
}
