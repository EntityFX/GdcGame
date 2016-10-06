using System;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class CustomJsonDeserializer : IDeserializer
    {
        static readonly Lazy<CustomJsonDeserializer> lazyInstance =
            new Lazy<CustomJsonDeserializer>(() => new CustomJsonDeserializer());

        public static CustomJsonDeserializer Default
        {
            get { return lazyInstance.Value; }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}