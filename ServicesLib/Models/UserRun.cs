using Microsoft.WindowsAzure.MobileServices;
using ServicesLib.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLib.Models
{
    public class UserRun
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AcceptedChallengeId { get; set; }

        public DateTime StartTime { get; set; }

        [DataMemberJsonConverter(ConverterType = typeof(CheckInListConverter))]
        public List<DateTime> CheckInList { get; set; }
    }
}
