using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Data.Json;

namespace ServicesLib.Models.Converters
{
    public class LocationListConverter : IDataMemberJsonConverter
    {
        private static readonly IJsonValue NullJson = JsonValue.Parse("null");

        public object ConvertFromJson(IJsonValue value)
        {
            List<Location> result = null;

            if (value != null && value.ValueType == JsonValueType.String)
            {
                string valueInStr = value.GetString();
                var locationPairs = valueInStr.Split(';');
                if (locationPairs != null)
                {
                    result = new List<Location>();

                    foreach (var locationPair in locationPairs)
                    {
                        if (locationPair.Length > 0)
                        {
                            var latLong = locationPair.Split(',');

                            result.Add(new Location()
                                {
                                    Latitude = Convert.ToDouble(latLong[0]),
                                    Longitude = Convert.ToDouble(latLong[1]),
                                });
                        }
                    }
                }
            }

            return result;
        }

        public IJsonValue ConvertToJson(object instance)
        {
            if (instance is List<Location>)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var loc in (instance as List<Location>))
                {
                    sb.Append(String.Format(";{0},{1}", loc.Latitude, loc.Longitude));
                }

                return JsonValue.CreateStringValue(sb.ToString());
            }
            else
            {
                return NullJson;
            }
        }
    }
}
