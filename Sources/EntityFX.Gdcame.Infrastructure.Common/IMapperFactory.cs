using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IMapperFactory
    {
        IMapper<TSource, TDestination> Build<TSource, TDestination>(string mapperMark = null)
            where TSource : class
            where TDestination : class;
    }
}
