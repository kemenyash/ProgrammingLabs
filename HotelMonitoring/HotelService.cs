using HotelMonitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelMonitoring
{
    internal class HotelService
    {
        public void GetStatByHotelRoom(IEnumerable<Visitor> visitors, int roomNumber)
        {
            int previousYear = DateTime.Now.Year - 1;

            var visitorsInRoomLastYear = visitors
                .Where(v => v.RoomNumber == roomNumber && v.SettlementTime.Year == previousYear)
                .Select(v => (v.EvictionTime - v.SettlementTime).TotalDays);

            int countVisitorsLastYearVisitors = 0;
            if (visitorsInRoomLastYear.Any())
            { 
                countVisitorsLastYearVisitors = visitorsInRoomLastYear.Count(); 
            }

            Console.WriteLine($"Count of last year visitors: {countVisitorsLastYearVisitors}");
        }


        public void GetFloorsWithSmallAmountRooms(List<Room> rooms)
        {

            var floorsWithRoomCount = rooms
                .GroupBy(r => r.Floor)
                .Select(g => new { Floor = g.Key, RoomCount = g.Count() })
                .OrderBy(f => f.RoomCount)
                .ToList();

            int minRoomCount = floorsWithRoomCount.First().RoomCount;

            var floors = floorsWithRoomCount
                .Where(f => f.RoomCount == minRoomCount)
                .Select(f => f.Floor)
                .ToList();

            Console.WriteLine($"Floors with small amount rooms: {string.Join(", ", floors)}");
        }


        public void CalculatePaymentForCurrentVisitors(List<Visitor> visitors, List<Room> rooms)
        {
            DateTime currentDate = DateTime.Now;

            var currentVisitors = visitors.Where(v => v.EvictionTime > currentDate);

            foreach (var visitor in currentVisitors)
            {
                var room = rooms.FirstOrDefault(r => r.RoomNumber == visitor.RoomNumber);

                if (room != null)
                {
                    int daysStayed = (currentDate - visitor.SettlementTime).Days;

                    decimal totalPayment = daysStayed * room.CostPerDay;

                    Console.WriteLine($"Visitor {visitor.FullName} in room {visitor.RoomNumber} must pay {totalPayment} UAH for {daysStayed} days of stay.");

                }
            }
        }
    }
}
