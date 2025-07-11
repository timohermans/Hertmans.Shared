namespace Hertmans.Shared.Auth.Services
{
    internal interface ITokenRetriever
    {
        Task<string?> GetAccessTokenAsync();
    }
}