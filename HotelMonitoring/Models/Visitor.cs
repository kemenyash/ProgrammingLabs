using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMonitoring.Models
{
    public class Visitor
    {
        public string FullName { get; set; }
        public DateTime ArivingTime { get; set; }
        public int RoomNumber { get; set; }
        public DateTime SettlementTime { get; set; }
        public DateTime EvictionTime { get; set; }
    }
}
