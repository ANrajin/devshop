using devshop.api.Contexts;
using devshop.api.Cores.Adapters;
using devshop.api.Cores.Contracts;
using devshop.api.Cores.UnitOfWorks;
using devshop.api.Cores.Utilities;
using devshop.api.Cores.Utilities.FileHandler;
using devshop.api.Features.Auths.Services;
using devshop.api.Features.Books.Repositories;
using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;
using devshop.api.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace devshop.api.Configs.dependencies
{
    public class ApplicationServicesInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services,
            IConfiguration configuration)
        {
            const string connectionStrName = "DevShopDb";
            var connectionString = configuration.GetConnectionString(connectionStrName);

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<EntityInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var entityInterceptor = sp.GetService<EntityInterceptor>()!;

                options.UseSqlServer(connectionString)
                    .AddInterceptors(entityInterceptor);
            });

            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUnitOfWorks, UnitOfWorks>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IFileHandlerService, FileHandlerService>();

            services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<ISigninManagerAdapter, SigninManagerAdapter>();
            services.AddScoped<ISigninManagerService, SigninManagerService>();

            services.AddScoped<BooksRequestHandler>();
        }
    }
}
