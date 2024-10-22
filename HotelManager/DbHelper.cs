using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HotelManager
{
    public class DbHelper
    {
        public bool IsInitializated { get; private set; }

        public IEnumerable<Floor> Floors
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Floors.ToList();
                }
            }
        }

        public IEnumerable<Duration> Durations
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Durations
                        .Include(g => g.Guest)
                        .Include(r => r.Room)
                        .Include (r => r.Room.Floor)
                        .ToList();
                }
            }
        }

        public IEnumerable<Guest> Guests
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Guests.ToList();
                }
            }
        }

        public IEnumerable<Room> Rooms
        {
            get
            {
                using (var dataContext = new DataContext())
                {
                    return dataContext.Rooms
                        .Include(f => f.Floor)
                        .ToList();
                }
            }
        }

        public DbHelper(bool isMockNeeded) 
        {
            if (isMockNeeded) Initialization().ConfigureAwait(false);
            else IsInitializated = true;
        }

        #region Initialization

        private async Task Initialization()
        {
            var floors = new List<Floor>();
            for(var i = 1; i < 10; i++)
            {
                floors.Add(new Floor
                {
                    Number = i
                });
            }

            using (var dataContext = new DataContext())
            {
                await dataContext.Floors.AddRangeAsync(floors);
                await dataContext.SaveChangesAsync();
            }

            await RoomsInit(floors);
        }

        private async Task RoomsInit(List<Floor> floors)
        {
            var costsViaGuestsCount = new Dictionary<int, decimal[]>
            {
                { 1, new decimal[] { 500, 1000, 1500, 2000 } }, //одномісні
                { 2, new decimal[]{ 1000, 1500, 2000, 2500, 3000 } }, //двомісні
                { 3, new decimal[]{ 2400, 3000, 3600, 4000 } } //трьохмісні
            };

            var rooms = new List<Room>();
            foreach(var floor in floors)
            {
                foreach(var guestsCountOnRoom in costsViaGuestsCount.Keys)
                {
                    var costs = costsViaGuestsCount[guestsCountOnRoom];
                    for(int i = 0; i < costs.Length; i++)
                    {
                        rooms.Add(new Room 
                        {
                            CostPerNight = costs[i],
                            SettlementTime = new TimeSpan(11, 0, 0),
                            EvictionTime = new TimeSpan(12, 0, 0),
                            FloorId = floor.Id,
                            GuestsCount = guestsCountOnRoom,
                            RoomNumber = i + 1
                        });
                    }
                }
            }

            using(var dataContext = new DataContext())
            {
                await dataContext.Rooms.AddRangeAsync(rooms);
                await dataContext.SaveChangesAsync();
            }

            await GuestsInit(rooms);
        }

        private async Task GuestsInit(List<Room> rooms)
        {
            var guests = new List<Guest>
            {
                new Guest { FirstName = "Serhiy", LastName = "Vasylenko" },
                new Guest { FirstName = "Andriy", LastName = "Petrenko" },
                new Guest { FirstName = "Olena", LastName = "Ivanova" },
                new Guest { FirstName = "Vasyl", LastName = "Shevchenko"},
                new Guest { FirstName = "Iryna", LastName = "Bondar"},
                new Guest { FirstName = "Maksym", LastName = "Moroz" },
                new Guest { FirstName = "Kateryna", LastName = "Kravets" },
                new Guest { FirstName = "Oleh", LastName = "Sydorenko" },
                new Guest { FirstName = "Svitlana", LastName = "Dmytrenko"},
                new Guest { FirstName = "Mykhailo", LastName = "Zaitsev" },
                new Guest { FirstName = "Natalia", LastName = "Horbunova" }
            };

            using(var dataContext = new DataContext())
            {
                await dataContext.Guests.AddRangeAsync(guests);
                await dataContext.SaveChangesAsync();
            }

            await DurationsInit(guests, rooms);
        }

        private async Task DurationsInit(List<Guest> guests, List<Room> rooms)
        {
            var durations = new List<Duration>();
            var daysOnRoom = 1;
            int arrivalIterator = 0;

            foreach (var room in rooms)
            {
                foreach (var guest in guests)
                {
                    if (daysOnRoom > 4) daysOnRoom = 1;
                    if (arrivalIterator == -3) arrivalIterator = 0;
                    arrivalIterator--;

                    var arrival = DateTime.Today.AddDays(arrivalIterator) + room.SettlementTime;
                    var eviction = arrival + TimeSpan.FromDays(daysOnRoom);
                    daysOnRoom++;


                    durations.Add(new Duration
                    {
                        Arrival = arrival.ToUniversalTime(),
                        Eviction = eviction.ToUniversalTime(),
                        GuestId = guest.Id,
                        RoomId = room.Id
                    });
                }
            }

            using (var dataContext = new DataContext())
            {
                await dataContext.Durations.AddRangeAsync(durations);
                await dataContext.SaveChangesAsync();
            }

            var randomMockDurations = new List<Duration>();
            using (var dataContext = new DataContext())
            {
                Random random = new Random();

                var randomDurations = dataContext.Durations.ToList();
                randomDurations = randomDurations.OrderBy(x => random.Next()).ToList();

                var durationsWithPreviousYear = randomDurations.Take(100).ToList();
                var durationsWithoutEviction = randomDurations.Skip(100).Take(200).ToList();

                foreach (var duration in durationsWithPreviousYear)
                {
                    if (duration.Eviction == DateTime.MinValue) continue;
                    duration.Arrival = duration.Arrival.AddYears(-1);
                    duration.Eviction = duration.Eviction.AddYears(-1);
                }

                foreach (var duration in durationsWithoutEviction) duration.Eviction = DateTime.MinValue;

                randomMockDurations.AddRange(durationsWithPreviousYear);
                randomMockDurations.AddRange(durationsWithoutEviction);
            }

            using (var dataContext = new DataContext())
            {
                dataContext.Durations.UpdateRange(randomMockDurations);
                await dataContext.SaveChangesAsync();
            }


           IsInitializated = true;
        }

        #endregion

    }
}
