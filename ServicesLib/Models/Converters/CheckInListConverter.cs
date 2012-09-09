using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace ServicesLib.Models.Converters
{
    public class CheckInListConverter : IDataMemberJsonConverter
    {
        private static readonly IJsonValue NullJson = JsonValue.Parse("null");

        public object ConvertFromJson(IJsonValue value)
        {
            List<DateTime> result = null;

            if (value != null && value.ValueType == JsonValueType.String)
            {
                string valueInStr = value.GetString();
                var checkIns = valueInStr.Split(';');
                if (checkIns != null)
                {
                    result = new List<DateTime>();

                    foreach (var checkIn in checkIns)
                    {
                        result.Add(Convert.ToDateTime(checkIn));
                    }
                }
            }

            return result;
        }

        public IJsonValue ConvertToJson(object instance)
        {
            if (instance is List<DateTime>)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var time in (instance as List<DateTime>))
                {
                    sb.Append(String.Format(";{0}", time.Ticks));
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
