namespace Hertmans.Shared.Auth.Services
{
    /// <summary>
    /// TokenProvider interface. Use it to set the token from a place where httpcontext is available
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Bearer token used to call API
        /// </summary>
        string? AccessToken { get; }

        /// <summary>
        /// Set token
        /// </summary>
        /// <param name="token">Token pulled from authorization header provider or http context</param>
        void SetToken(string token);
    }
}