namespace ValidatorGeocoder.Models
{
    public class AddressMeta
    {
        public string record_type { get; set; }
        public string zip_type { get; set; }
        public string county_fips { get; set; }
        public string county_name { get; set; }
        public string carrier_route { get; set; }
        public string congressional_district { get; set; }
        public string rdi { get; set; }
        public string elot_sequence { get; set; }
        public string elot_sort { get; set; }
    }
}
