using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
    [DataContract]
    public class RatingHistory
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public DateTime Data { get; set; }

        [DataMember]
        public int ManualStepsCount { get; set; }

        [DataMember]
        public decimal TotalEarned { get; set; }

        [DataMember]
        public int RootCounter { get; set; }
    }
}
