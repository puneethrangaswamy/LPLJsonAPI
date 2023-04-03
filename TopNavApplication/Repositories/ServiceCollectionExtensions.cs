namespace TopNavApplication.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection, IConfigurationRoot configRoot)
        {
            collection.Configure<ConnectionStringsOptions>(configRoot.GetSection("ConnectionStrings"));

            collection.AddTransient<IUserRepository, DefaultUserRepository>();
            collection.AddTransient<IMenuRepository, DefaultMenuRepository>();

            return collection;
        }
    }
}
