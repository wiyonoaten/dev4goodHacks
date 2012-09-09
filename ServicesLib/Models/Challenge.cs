using Microsoft.WindowsAzure.MobileServices;
using ServicesLib.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLib.Models
{
    public class Challenge
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatorUserId { get; set; }
        
        public DateTime ExpiryTime { get; set; }

        public decimal Wager { get; set; }

        [DataMemberJsonConverter(ConverterType = typeof(LocationListConverter))]
        public List<Location> LocationList { get; set; }
    }
}
