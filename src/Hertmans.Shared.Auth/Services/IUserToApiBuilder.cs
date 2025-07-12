namespace Hertmans.Shared.Auth.Services;

public static partial class UserToApiClientExtensions
{
    public interface IUserToApiBuilder
    {
        /// <summary>
        /// Register types for a tokenprovider in Blazor.
        /// This will add an ITokenProvider to the service collection.
        /// Inject it in App.razor, fetch access token from httpcontext and set the token.
        /// </summary>
        /// <returns></returns>
        IUserToApiBuilder WithBlazorTokenProvider();

        /// <summary>
        /// Add an HTTP client to the service collection. Automatically add access token handling as well
        /// </summary>
        /// <typeparam name="TInterface">The interface type of the HTTP client.</typeparam>
        /// <typeparam name="T">The implementation type of the HTTP client.</typeparam>
        /// <returns>The builder to easily call AddHttpClient again.</returns>
        IUserToApiBuilder AddHttpClient<TInterface, T>()
            where TInterface : class
            where T : class, TInterface;

        /// <summary>
        /// Add an HTTP client to the service collection. Automatically add access token handling as well
        /// </summary>
        /// <typeparam name="T">typeof HTTP client</typeparam>
        /// <returns>the builder to easily call AddHttpClient again</returns>
        IUserToApiBuilder AddHttpClient<T>() where T : class;

        /// <summary>
        /// Add a Refit Http Client to the service collection.
        /// Adds access token handling as well.
        /// Note that TokenProvider does not work with Refit somehow!
        /// A simple IHttpContextAccessor works just fine somehow
        /// </summary>
        /// <typeparam name="T">typeof Refit client interface</typeparam>
        /// <returns>The builder</returns>
        IUserToApiBuilder AddRefitClient<T>() where T : class;
    }
}