namespace ValidatorGeocoder.Models
{
    public class ValidatorExport
    {
        public string FileName { get; set; }
        public ValidatorResults[] SavedProperties { get; set; }
        public Address[] RejectedProperties { get; set; }
    }
}
