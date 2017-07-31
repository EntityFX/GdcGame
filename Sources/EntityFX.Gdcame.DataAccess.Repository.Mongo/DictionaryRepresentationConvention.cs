namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson.Serialization.Options;

    class DictionaryRepresentationConvention : ConventionBase, IMemberMapConvention
    {
        private readonly DictionaryRepresentation _dictionaryRepresentation;
        public DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation)
        {
            this._dictionaryRepresentation = dictionaryRepresentation;
        }
        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetSerializer(this.ConfigureSerializer(memberMap.GetSerializer()));
        }
        private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer)
        {
            var dictionaryRepresentationConfigurable = serializer as IDictionaryRepresentationConfigurable;
            if (dictionaryRepresentationConfigurable != null)
            {
                serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(this._dictionaryRepresentation);
            }

            var childSerializerConfigurable = serializer as IChildSerializerConfigurable;
            return childSerializerConfigurable == null
                ? serializer
                : childSerializerConfigurable.WithChildSerializer(this.ConfigureSerializer(childSerializerConfigurable.ChildSerializer));
        }
    }
}
