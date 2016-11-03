using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;


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

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }

    public class CustomJsonSerializer : ISerializer
    {
        private Newtonsoft.Json.JsonSerializer serializer;

        public CustomJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public string ContentType
        {
            get { return "application/json"; } // Probably used for Serialization?
            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    
                    serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }


        public static CustomJsonSerializer Default
        {
            get
            {
                return new CustomJsonSerializer(new Newtonsoft.Json.JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }
    }
}