using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorGeocoder.Models
{
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
}
