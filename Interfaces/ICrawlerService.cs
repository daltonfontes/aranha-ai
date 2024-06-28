namespace aranha.Interfaces
{
    public interface ICrawlerService
    {
        Task<string> NavigateToUrlAsync(string url);
    }
}
