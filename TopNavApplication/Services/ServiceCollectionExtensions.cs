namespace TopNavApplication.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection collection, IConfigurationRoot configRoot)
        {
            collection.Configure<JwtTokenServiceOptions>(configRoot.GetSection("JwtTokenService"));

            collection.AddTransient<IAuthenticationService, SimpleAuthenticationService>()
                      .AddTransient<ITokenService, JwtTokenService>()
                      .AddTransient<IMenuService, DefaultMenuService>();

            return collection;
        }
    }
}
