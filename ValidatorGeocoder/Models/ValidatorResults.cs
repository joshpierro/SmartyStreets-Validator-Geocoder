namespace ValidatorGeocoder.Models
{
    public class ValidatorResults
    {
        public string Id { get; set; }
        public int USPScandidate_index { get; set; }
        public string USPSdelivery_line_1 { get; set; }
        public string USPSlast_line { get; set; }
        public string USPSprimary_number { get; set; }
        public string USPSstreet_predirection { get; set; }
        public string USPSstreet_name { get; set; }
        public string USPSstreet_suffix { get; set; }
        public string USPScity_name { get; set; }
        public string USPSstate_abbreviation { get; set; }
        public string USPSzipcode { get; set; }
        public string USPSplus4_code { get; set; }
        public string USPSrecord_type { get; set; }
        public string USPSzip_type { get; set; }
        public string USPScounty_fips { get; set; }
        public string USPScounty_name { get; set; }
        public string USPScarrier_route { get; set; }
        public string USPScongressional_district { get; set; }
        public string USPSrdi { get; set; }
        public string USPSelot_sequence { get; set; }
        public string USPSelot_sort { get; set; }
        public string USPSdpv_match_code { get; set; }
        public string USPSdpv_footnotes { get; set; }
        public string USPSdpv_cmra { get; set; }
        public string USPSdpv_vacant { get; set; }
        public string USPSactive { get; set; }
        public string USPSfootnotes { get; set; }
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
