using Hertmans.Shared.Auth.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace Hertmans.Shared.Auth.Services;

public static partial class UserToApiClientExtensions
{
    internal class UserToApiBuilder : IUserToApiBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IApiClientOptions _options;
        private readonly string _authenticationScheme;

        private bool IsKiotaBaseAdded = false;

        private UserToApiBuilder(IServiceCollection services, IApiClientOptions options,
            string authenticationScheme)
        {
            _services = services;
            _options = options;
            _authenticationScheme = authenticationScheme;
        }

        public static UserToApiBuilder New<T>(IServiceCollection services, IConfiguration config,
            string sectionName, string authenticationScheme = "OpenIdConnect") where T : class, IApiClientOptions, new()
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            var settings = new T();
            config.GetSection(sectionName).Bind(settings, opts => opts.ErrorOnUnknownConfiguration = true);

            return new UserToApiBuilder(services, settings, authenticationScheme);
        }

        /// <inheritdoc />
        public IUserToApiBuilder WithBlazorTokenProvider()
        {
            _services.AddCircuitServicesAccessor();
            _services.AddCircuitTokenProvider();
            
            return this;
        }

        /// <inheritdoc />
        public IUserToApiBuilder AddHttpClient<TInterface, T>()
            where TInterface : class
            where T : class, TInterface
        {
            _services.AddHttpClient<TInterface, T>(WithBaseAddress)
                .AddHttpMessageHandler(WithApiBearerToken);
            return this;
        }

        /// <inheritdoc />
        public IUserToApiBuilder AddKiotaClient<T>() where T : class
        {
            if (!IsKiotaBaseAdded)
            {
                _services.AddHttpClient<IRequestAdapter, HttpClientRequestAdapter>(WithBaseAddress);
                _services.AddScoped<IAuthenticationProvider, KiotaAuthProvider>();
                IsKiotaBaseAdded = true;
            }

            _services.AddScoped<T>();
            return this;
        }

        /// <inheritdoc />
        public IUserToApiBuilder AddHttpClient<T>() where T : class
        {
            _services.AddHttpClient<T>(WithBaseAddress)
                .AddHttpMessageHandler(WithApiBearerToken);
            return this;
        }

        private void WithBaseAddress(HttpClient client) =>
            client.BaseAddress = new Uri(_options.BaseUrl);

        private DelegatingHandler WithApiBearerToken(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<UserToApiBuilder>>();
            var factory = new TokenRetrieverFactory(services, _authenticationScheme);
            var tokenRetriever = factory.CreateRetriever();

            return new ApiBearerTokenHandler(logger, tokenRetriever);
        }
    }
}