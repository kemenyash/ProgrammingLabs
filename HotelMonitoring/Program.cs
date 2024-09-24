using HotelMonitoring;
using HotelMonitoring.Models;

#region Initialization
var hotelService = new HotelService();

var visitors = new List<Visitor>
{
    new Visitor { FullName = "Serhiy Vasylenko", RoomNumber = 101, SettlementTime = new DateTime(2023, 9, 12), EvictionTime = new DateTime(2023, 9, 14) },
    new Visitor { FullName = "Andriy Petrenko", RoomNumber = 101, SettlementTime = new DateTime(2023, 9, 12), EvictionTime = new DateTime(2023, 9, 15)  },
    new Visitor { FullName = "Olena Ivanova", RoomNumber = 102, SettlementTime = new DateTime(2024, 9, 14), EvictionTime = new DateTime(2024, 10, 15) },
    new Visitor { FullName = "Vasyl Shevchenko", RoomNumber = 201, SettlementTime = new DateTime(2024, 9, 17), EvictionTime = new DateTime(2024, 10, 10) },
    new Visitor { FullName = "Iryna Bondar", RoomNumber = 202, SettlementTime = new DateTime(2024, 9, 16), EvictionTime = new DateTime(2024, 10, 5) },
    new Visitor { FullName = "Maksym Moroz", RoomNumber = 301, SettlementTime = new DateTime(2024, 9, 11), EvictionTime = new DateTime(2024, 10, 12) },
    new Visitor { FullName = "Kateryna Kravets", RoomNumber = 302, SettlementTime = new DateTime(2024, 9, 10), EvictionTime = new DateTime(2024, 10, 15) },
    new Visitor { FullName = "Oleh Sydorenko", RoomNumber = 303, SettlementTime = new DateTime(2024, 9, 17), EvictionTime = new DateTime(2024, 10, 20) },
    new Visitor { FullName = "Svitlana Dmytrenko", RoomNumber = 401, SettlementTime = new DateTime(2024, 9, 11), EvictionTime = new DateTime(2024, 10, 25) },
    new Visitor { FullName = "Mykhailo Zaitsev", RoomNumber = 402, SettlementTime = new DateTime(2024, 9, 15), EvictionTime = new DateTime(2024, 10, 30) },
    new Visitor { FullName = "Natalia Horbunova", RoomNumber = 403, SettlementTime = new DateTime(2024, 9, 14), EvictionTime = new DateTime(2024, 11, 5) }
};


var rooms = new List<Room>
{
    new Room { RoomNumber = 101, Floor = 1, PlacesCount = 2, CostPerDay = 1000 },
    new Room { RoomNumber = 102, Floor = 1, PlacesCount = 3, CostPerDay = 1200 },
    new Room { RoomNumber = 201, Floor = 2, PlacesCount = 2, CostPerDay = 1300 },
    new Room { RoomNumber = 202, Floor = 2, PlacesCount = 1, CostPerDay = 800 },
    new Room { RoomNumber = 301, Floor = 3, PlacesCount = 4, CostPerDay = 1500 },
    new Room { RoomNumber = 302, Floor = 3, PlacesCount = 2, CostPerDay = 1100 },
    new Room { RoomNumber = 303, Floor = 3, PlacesCount = 3, CostPerDay = 1200 },
    new Room { RoomNumber = 401, Floor = 4, PlacesCount = 2, CostPerDay = 1300 },
    new Room { RoomNumber = 402, Floor = 4, PlacesCount = 1, CostPerDay = 900 },
    new Room { RoomNumber = 403, Floor = 4, PlacesCount = 3, CostPerDay = 1400 }
};
#endregion

hotelService.GetStatByHotelRoom(visitors, 101);
Console.WriteLine();
hotelService.GetFloorsWithSmallAmountRooms(rooms);
Console.WriteLine();
hotelService.CalculatePaymentForCurrentVisitors(visitors, rooms);

