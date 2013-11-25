using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ValidatorGeocoder.Models;

namespace ValidatorGeocoder.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
 
            return View();
        }


        public ActionResult ProcessAddresses(string tableData,string smartyId,string smartyToken)
        {
            var tableAddresses = HttpUtility.UrlDecode(tableData);

            var addresses = JsonConvert.DeserializeObject<List<Address>>(tableAddresses);
            var validatedAddresses = Validate(addresses, smartyId, smartyToken);
            var geocodedAddresses = Geocode(validatedAddresses);
            var normalizedAddresses = NormalizeAddresses(geocodedAddresses);

            var rejectedAddresses = addresses.Where(a => !(from o in validatedAddresses select o.Id).Contains(a.Id)).ToList();
            

            var model = new ValidatorExport
            {
                FileName = "ValidatedProperties.xls",
                SavedProperties = normalizedAddresses.ToArray(),
                RejectedProperties = rejectedAddresses.ToArray()
            };


            return View(model);

        }


         [HttpPost]
        public List<ValidatedAddress> Validate(List<Address> addresses, string smartyId,string smartyToken)
         {

             var validatedList = new List<ValidatedAddress>();

             foreach (var addr in addresses)
             {
                 string Id = addr.Id;
                 string address = addr.StreetAddress;
                 string city = addr.City;
                 string state = addr.State;
                 string zip = addr.Zipcode;


                 string validationUrl = string.Format("https://api.smartystreets.com/street-address?street={0}&city={1}&state={2}&zipcode={3}&candidates=5&auth-id={4}&auth-token={5}", address, city, state, zip, smartyId, smartyToken);
                 validationUrl = validationUrl.Replace("#", "");
                 
                 using (var client = new WebClient())
                 {
                     using (var stream = client.OpenRead(validationUrl))
                     {

                         var reader = new StreamReader(stream);
                         var response = reader.ReadToEnd();

                         var validatedAddress = JsonConvert.DeserializeObject<List<ValidatedAddress>>(response);
                         
                         foreach (var validatedAddr in validatedAddress)
                         {
                             validatedAddr.Id = addr.Id;
                             validatedList.Add(validatedAddr);
                         }
                
                     }
                 }



             }


             return validatedList; 
         }


         public List<ValidatedAddress> Geocode(List<ValidatedAddress> addr)
         {

  

             var geocodedResults = new List<ValidatedAddress>();

             foreach (var validatedAddress in addr)
             {


                 string geocodeUrl =
                     string.Format(
                         "http://maps.googleapis.com/maps/api/geocode/json?address={0} {1} {2} {3}, {4},{5},{6}&sensor=true",
                         validatedAddress.components.primary_number,
                         validatedAddress.components.street_predirection,
                         validatedAddress.components.street_name,
                         validatedAddress.components.street_suffix,
                         validatedAddress.components.city_name,
                         validatedAddress.components.state_abbreviation,
                         validatedAddress.components.zipcode
                         );


                 using (var client = new WebClient())
                 {

                     using (var stream = client.OpenRead(geocodeUrl))
                     {
                         var reader = new StreamReader(stream);
                         var response = reader.ReadToEnd();
                         var googleGeocode = JsonConvert.DeserializeObject<GoogleGeocode>(response);


                         double lat;
                         double lng;

                         var addressNumber = new AddressComponent();
                         var streetName = new AddressComponent();
                         var muni = new AddressComponent();
                         var subMuni = new AddressComponent();
                         var stateName = new AddressComponent();
                         var zipCode = new AddressComponent();
                         var countyName = new AddressComponent();

                          
                        
                         if (googleGeocode.Status == "ZERO_RESULTS")
                         {
                             validatedAddress.GoogleLatitude = 0.00;
                             validatedAddress.GoogleLongitude = 0.00;
                             validatedAddress.GoogleLocationType = "";

                             addressNumber.long_name = "Not Validated - Check Address";
                             streetName.long_name = "";
                             muni.long_name = "Not Validated - Check Address";
                             subMuni.long_name = "Not Validated - Check Address";
                             stateName.long_name = "Not Validated - Check Address";
                             zipCode.long_name = "Not Validated - Check Address";
                             countyName.long_name = "Not Validated - Check Address";

                         }
                         else
                         {


                             validatedAddress.GoogleLatitude = googleGeocode.results == null ? 0.0 : googleGeocode.results[0].geometry.location.lat;
                             validatedAddress.GoogleLongitude = googleGeocode.results == null ? 0.0 : googleGeocode.results[0].geometry.location.lng;
                             validatedAddress.GoogleLocationType = googleGeocode.results == null ? "" : googleGeocode.results[0].geometry.location_type;
                            
                             
                             addressNumber = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("street_number"));
                             streetName = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("route"));

                             muni = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("locality"));
                             subMuni = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("sublocality"));
                             stateName = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("administrative_area_level_1"));
                             zipCode = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("postal_code"));
                             countyName = googleGeocode.results[0].address_components.FirstOrDefault(m => m.types.Contains("administrative_area_level_2"));


                             validatedAddress.GoogleStreetAddress = addressNumber == null || streetName == null
                                                                  ? "Not Validated - Check Address"
                                                                  : string.Format("{0} {1}", addressNumber.long_name,
                                                                                  streetName.long_name);

                             validatedAddress.GoogleCity = muni != null
                                                                 ? muni.long_name
                                                                 : subMuni != null
                                                                       ? subMuni.long_name
                                                                       : "Not Validated - Check Address";

                             validatedAddress.GoogleState = stateName == null
                                                                  ? "Not Validated - Check Address"
                                                                  : stateName.long_name;

                             validatedAddress.GoogleZip = zipCode == null
                                                                ? "Not Validated - Check Address"
                                                                : zipCode.long_name;

                             validatedAddress.GoogleCounty = countyName == null
                                                                   ? "Not Validated - Check Address"
                                                                   : countyName.long_name;


                         }


                         

     
                     }


                 }


             geocodedResults.Add(validatedAddress);
             Thread.Sleep(500); 

             }

  
             return geocodedResults;
         }




         public List<ValidatorResults> NormalizeAddresses(List<ValidatedAddress> validatorResultsList)
         {

             var normalizedResultsList = new List<ValidatorResults>();

             foreach (var validatedAddress in validatorResultsList)
             {
                 var validatorResults = new ValidatorResults();
                
                 validatorResults.Id = validatedAddress.Id; 
                 validatorResults.GoogleCity = validatedAddress.GoogleCity;
                 validatorResults.GoogleCounty = validatedAddress.GoogleCounty;
                 validatorResults.GoogleLatitude = validatedAddress.GoogleLatitude;
                 validatorResults.GoogleLocationType = validatedAddress.GoogleLocationType;
                 validatorResults.GoogleLongitude = validatedAddress.GoogleLongitude;
                 validatorResults.GoogleState = validatedAddress.GoogleState;
                 validatorResults.GoogleStreetAddress = validatedAddress.GoogleStreetAddress;
                 validatorResults.GoogleZip = validatedAddress.GoogleZip;
                 validatorResults.USPSactive = validatedAddress.analysis.active;
                 validatorResults.USPScandidate_index = validatedAddress.candidate_index;
                 validatorResults.USPScarrier_route = validatedAddress.metadata.carrier_route;
                 validatorResults.USPScity_name = validatedAddress.components.city_name;
                 validatorResults.USPScongressional_district = validatedAddress.metadata.congressional_district;
                 validatorResults.USPScounty_fips = validatedAddress.metadata.county_fips;
                 validatorResults.USPScounty_name = validatedAddress.metadata.county_name;
                 validatorResults.USPSdelivery_line_1 = validatedAddress.delivery_line_1;
                 validatorResults.USPSdpv_cmra = validatedAddress.analysis.dpv_cmra;
                 validatorResults.USPSdpv_footnotes = validatedAddress.analysis.dpv_footnotes;
                 validatorResults.USPSdpv_match_code = validatedAddress.analysis.dpv_match_code;
                 validatorResults.USPSdpv_vacant = validatedAddress.analysis.dpv_vacant;
                 validatorResults.USPSelot_sequence = validatedAddress.metadata.elot_sequence;
                 validatorResults.USPSelot_sort = validatedAddress.metadata.elot_sort;
                 validatorResults.USPSfootnotes = validatedAddress.analysis.footnotes;
                 validatorResults.USPSlast_line = validatedAddress.last_line;
                 validatorResults.USPSplus4_code = validatedAddress.components.plus4_code;
                 validatorResults.USPSprimary_number = validatedAddress.components.primary_number;
                 validatorResults.USPSrdi = validatedAddress.metadata.rdi;
                 validatorResults.USPSrecord_type = validatedAddress.metadata.record_type;
                 validatorResults.USPSstate_abbreviation = validatedAddress.components.state_abbreviation;
                 validatorResults.USPSstreet_name = validatedAddress.components.street_name;
                 validatorResults.USPSstreet_predirection = validatedAddress.components.street_predirection;
                 validatorResults.USPSstreet_suffix = validatedAddress.components.street_suffix;
                 validatorResults.USPSzip_type = validatedAddress.metadata.zip_type;
                 validatorResults.USPSzipcode = validatedAddress.components.zipcode;

                 normalizedResultsList.Add(validatorResults);
             }

                                                                                                  
          

             return normalizedResultsList; 

         }


 
    }
}
