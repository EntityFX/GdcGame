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
        public decimal Day { get; set; }
        [DataMember]
        public decimal Week { get; set; }
        [DataMember]
        public decimal Total { get; set; }
    }
}
