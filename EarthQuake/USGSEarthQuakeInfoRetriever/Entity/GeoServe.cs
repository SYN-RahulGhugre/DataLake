using System.Collections.Generic;
using System.Runtime.Serialization;

namespace USGSEarthQuakeInfoRetriever.Entity
{
    public class GeoServe
    {
        [DataMember(Name = "cities")]
        public List<Location> Cities { get; set; }

        [DataMember(Name = "region")]
        public Region Region { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
        //other fields are there
        /*

        {"cities":[{"distance":25,"latitude":40.30627,"name":"Chester, California","direction":"WNW","population":2144,"longitude":-121.23191},{"distance":68,"latitude":39.81211,"name":"Magalia, California","direction":"N","population":11310,"longitude":-121.57831},{"distance":69,"latitude":40.17849,"name":"Red Bluff, California","direction":"ENE","population":14076,"longitude":-122.23583},{"distance":70,"latitude":40.41628,"name":"Susanville, California","direction":"W","population":17947,"longitude":-120.65301},{"distance":203,"latitude":39.1638,"name":"Carson City, Nevada","direction":"NW","population":55274,"longitude":-119.7674}],"auth":"NC","timezone":{"utcOffset":-420,"latitude":40.426833,"utcTime":"07/11/2016:14:43:03 UTC","time":"07/11/2016:07:43:03","shortName":"PDT","longitude":-121.483167,"longName":"Pacific Daylight Time"},"links":[{"text":"Additional earthquake information for California","url":"http://earthquake.usgs.gov/earthquakes/states/index.php?regionID=5"}],"region":{"country":"United States","state":"California"},"fe":{"number":36,"mediumName":"LASSEN PEAK AREA, CALIFORNIA","hds":"NORTH AMERICA;USA;CALIFORNIA;VOLCANO","spanishName":"AREA DEL PICO LASSEN, CALIFORNIA","shortName":"LASSEN PEAK AREA, CALIFORNIA","longName":"Lassen Peak area, California"},"tectonicSummary":{}}

        region":{"country":"United States","state":"California"},
         * 
         */

    }

}
