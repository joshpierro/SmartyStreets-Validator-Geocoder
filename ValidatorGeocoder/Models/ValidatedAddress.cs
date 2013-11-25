namespace ValidatorGeocoder.Models
{
    public class ValidatedAddress
    {
        public string Id { get; set; }
        public int input_index { get; set; }
        public int candidate_index { get; set; }
        public string delivery_line_1 { get; set; }
        public string last_line { get; set; }
        public AddressComponents components { get; set; }
        public AddressMeta metadata { get; set; }
        public AddressAnalysis analysis { get; set; }
        public double GoogleLatitude { get; set; }
        public double GoogleLongitude { get; set; }
        public string GoogleStreetAddress { get; set; }
        public string GoogleCity { get; set; }
        public string GoogleState { get; set; }
        public string GoogleZip { get; set; }
        public string GoogleCounty { get; set; }
        public string GoogleLocationType { get; set; }

    }
}