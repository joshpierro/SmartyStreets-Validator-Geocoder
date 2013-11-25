namespace ValidatorGeocoder.Models
{
    public class AddressComponents
    {
        public string primary_number { get; set; }
        public string street_predirection { get; set; }
        public string street_name { get; set; }
        public string street_suffix { get; set; }
        public string city_name { get; set; }
        public string state_abbreviation { get; set; }
        public string zipcode { get; set; }
        public string plus4_code { get; set; }
    }
}