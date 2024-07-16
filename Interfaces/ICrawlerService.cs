using Microsoft.AspNetCore.Mvc;

namespace aranha.Interfaces
{
    public interface ICrawlerService
    {
        Task<string> NavigateToUrlAsync(string url);
        Task<byte[]> GetScreenshotAsync();
    }
}
