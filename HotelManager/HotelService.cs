using DataStore;
using HotelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager
{
    public class HotelService
    {
        private readonly DbHelper dbHelper;

        public HotelService(DbHelper dbHelper) 
        {
            this.dbHelper = dbHelper;
        }

        public void GetStatByHotelRoomOnPreviousYear(int roomNumber)
        {
            /*
             * За умовами завдання нам потрібно вивести середню тривалість проживання
             * в готелі гостей, які були поселені в заданій кімнаті минулого року
             * 
             * Отримуємо з суміжої таблиці durations дані за минулий рік і з заданим номером
             */

            var durations = dbHelper.Durations.Where(x => x.Arrival.Year == DateTime.Now.Year - 1 && 
                                                          x.Room.RoomNumber == roomNumber);

            /*
             * На вибірці ми робимо виклик Select, який поверне IEnumberable зі значеннями
             * double в середині, так як умовою нашого Select'у є різниця дат виселення та поселення.
             * Так як обидві властивості є структурами DateTime, то їх різниця матиме таку ж саму
             * структуру, відповідно ми можемо на виході в наш IEnumerable записати значення
             * властивості TotalDays.
             * 
             * В результаті ми будемо мати список, в якому будуть записані всі тривалості
             * проживання гостей номеру за минулий рік
             */

            var totalDays = durations.Select(x => (x.Eviction - x.Arrival).TotalDays);


            /*
             * Маючи список тривалостей проживання ми за допомогою методу Average можемо
             * визначити середню тривалість проживання в визначеному номері за минулий рік
             * та вивести в консоль
             */
            double visitorsInRoomLastYearAverage = 0;
            if (totalDays.Any())
            {
                visitorsInRoomLastYearAverage = totalDays.Average();
            }

            Console.WriteLine($"Average length of stay in the room {roomNumber} on previous year: {Math.Round(visitorsInRoomLastYearAverage).ToString().Replace(",", ".")} days");
        }


        public void GetFloorsWithSmallAmountRooms()
        {

            /*
             * По умовам завдання нам потрібно виветси поверхи з найменшою кількістю кімнат, що
             * в свою чергу залежить від кількості ліжкомість в номері
             * 
             * 
             * Маючи відформатовану вибірку з поверхами та кількістю кімнат ми 
             * викликаємо на ній метод OrderBy, вказуючи, що впорядкування повинно відбуватись
             * за кількістю кімнат на поверсі в порядку від найменшої кількості до 
             * найбільшої (за-замовчуванням)
             */

            var rooms = dbHelper.Rooms;

            var floorsWithRoomCount = rooms
            .GroupBy(r => new { r.FloorId, r.Floor.Number })
            .Select(g => new
            {
                FloorId = g.Key.FloorId,
                FloorNumber = g.Key.Number,
                TotalGuestsCount = g.Sum(r => r.GuestsCount)
            })
            .OrderBy(f => f.TotalGuestsCount)
            .ToList();

            /*
             * Далі нам потрібно визначити і записати в змінну найменшу кількість
             * кімнат,  яка в принципі існує в готелі, для цього нам достатньо вибрати
             * найперший об'єкт з вибірки (так як вона і так попередньо впорядкована)
             * і його властивість TotalGuestsCount записати в змінну
             */

            int minRoomCount = floorsWithRoomCount.First().TotalGuestsCount;

            /*
             * Знаючи найменше число кімнат в готелі ми можемо зробити вибірку
             * поверхів, у яких саме стільки кімнат і вивести їх в консоль.
             * Для цього ми беремо вже упорядкований список floorsWithRoomCount
             * та робимо вибірку елементів, в яких кількість кімнат буде рівна
             * minRoomCount після чого форматуємо все з допомогою Select в
             * IEnumerable<int>, щоб можна було легко вивести список поверхів
             * в консолі з допомогою string.Join використовуючи сепараторну кому
             */

            var floors = floorsWithRoomCount
                .Where(f => f.TotalGuestsCount == minRoomCount)
                .Select(f => f.FloorNumber);

            Console.WriteLine($"Floors with small amount rooms: {string.Join(", ", floors)}");
        }


        public void CalculatePaymentForCurrentVisitors()
        {
            /*
             * За умовами завдання нам потрібно вивести кожному гостю готеля, який ще не
             * виселився, суму його заборгованості по теперішній день
             * 
             * Для того, щоб визначити зі списку гостей саме тих, які ще не виселились,
             * нам потрібно перебрати список гостей з таблиці durations, де дата виселення
             * ще поки не задана (так як це структура, то по-замовчуванню вона рівна DateTime.MinValue)
             * 
             */

            var durationsWithotEvictions = dbHelper.Durations.Where(x => x.Eviction == DateTime.MinValue);
            var debts = durationsWithotEvictions.Select(x =>
            {
                // Обчислюємо загальну кількість днів
                var totalDays = Math.Round((DateTime.Now - x.Arrival).TotalDays);

                // Обчислюємо загальну вартість
                var totalCost = (DateTime.Now.Day - x.Arrival.Day) * x.Room.CostPerNight;

                // Форматуємо рядок без десяткових частин, якщо число ціле
                return $"Debts for guest {x.Guest.FirstName} {x.Guest.LastName} " +
                       $"with total day living of {totalDays:0} day(s) on room with number {x.Room.RoomNumber} " +
                       $"on {x.Room.Floor.Number} floor is {totalCost:0} UAH";
            }).ToArray();



            Console.WriteLine(String.Join("\r\n", debts));

        }
    }
}