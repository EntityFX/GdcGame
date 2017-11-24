using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder UseGameUserStoreAdaptor(
            this IdentityBuilder builder
        ) => builder
            .AddDBUserStore()
            .AddDBRoleStore();

        private static IdentityBuilder AddDBUserStore(
            this IdentityBuilder builder
        )
        {

            builder.Services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(builder.UserType),
                typeof(GameUserStore)
            );

            return builder;
        }

        private static IdentityBuilder AddDBRoleStore(
            this IdentityBuilder builder
        )
        {

            builder.Services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(builder.RoleType),
                typeof(GameUserRoleStore)
            );

            return builder;
        }
    }
}