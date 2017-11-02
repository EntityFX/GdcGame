
namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class MapperFactory : IMapperFactory
    {
        private readonly IIocContainer _unityContainer;

        public MapperFactory(IIocContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IMapper<TSource, TDestination> Build<TSource, TDestination>(string mapperName = null)
            where TSource : class where TDestination : class
        {
            IMapper<TSource, TDestination> mapper;

            if (mapperName == null)
                mapper = _unityContainer.Resolve<IMapper<TSource, TDestination>>();
            else
                mapper = _unityContainer.Resolve<IMapper<TSource, TDestination>>(mapperName);

            return mapper;
        }
    }
}