using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLib.Models
{
    public class Challenge
    {
        public User Creator { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<Location> Location { get; set; }
    }
}
