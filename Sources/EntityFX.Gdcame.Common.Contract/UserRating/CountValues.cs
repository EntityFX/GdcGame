using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Common.Contract.UserRating
{
    [DataContract]
    public class CountValues
    {
        [DataMember]
        public double Day { get; set; }
        [DataMember]
        public double Week { get; set; }
        [DataMember]
        public double Total { get; set; }
    }
}
