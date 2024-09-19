using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMonitoring.Models
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public int Floor { get; set; }
        public int PlacesCount { get; set; }
        public decimal CostPerDay { get; set; }
    }
}
