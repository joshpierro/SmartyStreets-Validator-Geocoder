using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValidatorGeocoder.Models
{
    public class GoogleResult
    {
        public AddressComponent[] address_components { get; set; }
        public GoogleGeometry geometry { get; set; }
    }
}