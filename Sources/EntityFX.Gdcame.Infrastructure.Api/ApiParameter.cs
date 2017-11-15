namespace EntityFX.Gdcame.Infrastructure.Api
{
    public class ApiParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string ContentType { get; set; }
        public ApiParameterType Type { get; set; }
    }
}