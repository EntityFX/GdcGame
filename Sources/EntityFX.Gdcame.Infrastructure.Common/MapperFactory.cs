﻿using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public class MapperFactory : IMapperFactory
    {
        private readonly IUnityContainer _unityContainer;

        public MapperFactory(IUnityContainer unityContainer)
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