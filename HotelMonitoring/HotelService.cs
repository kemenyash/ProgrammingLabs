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
            /*
             * За умовами завдання нам потрібно вивести середню тривалість проживання
             * в готелі гостей, які були поселені в заданій кімнаті минулого року
             * 
             * Визначаємо рік, виходячи з того, що нам потрібно отримати число відвідувачів
             * номеру саме за минулий рік
             */
            int previousYear = DateTime.Now.Year - 1;

            /*
             * З списку visitors робимо вибірку за наступними критеріями:
             * 1) властивість RoomNumber повина відповідати параметру roomNumber, який ми передали
             * в метод;
             * 2) рік поселення, за який відповідає вкладена властивість Year, що належить 
             * властивості-структурі SettlementTime, повинна бути рівною змінній previousYear
             * 
             * На вибірці ми робимо виклик Select, який поверне IEnumberable зі значеннями
             * double в середині, так як умовою нашого Select'у є різниця дат виселення та поселення.
             * Так як обидві властивості є структурами DateTime, то їх різниця матиме таку ж саму
             * структуру, відповідно ми можемо на виході в наш IEnumerable записати значення
             * властивосты TotalDays.
             * 
             * В результаті ми будемо мати список, в якому будуть записані всі тривалості
             * проживання гостей номеру за минулий рік
             */

            var visitorsInRoomLastYear = visitors
                .Where(v => v.RoomNumber == roomNumber && v.SettlementTime.Year == previousYear)
                .Select(v => (v.EvictionTime - v.SettlementTime).TotalDays);

            /*
             * Маючи список тривалостей проживання ми за допомогою методу Average можемо
             * визначити середню тривалість проживання в визначеному номері за минулий рік
             * та вивести в консоль
             */


            double visitorsInRoomLastYearAverage = 0;
            if (visitorsInRoomLastYear.Any())
            {
                visitorsInRoomLastYearAverage = visitorsInRoomLastYear.Average();
            }

            Console.WriteLine($"Average length of stay in the room {roomNumber} on previous year: {visitorsInRoomLastYearAverage} days");
        }


        public void GetFloorsWithSmallAmountRooms(List<Room> rooms)
        {

            /*
             * По умовам завдання нам потрібно виветси поверхи з найменшою кількістю кімнат,
             * але так як у нас самого класу поверху немає (згідно ТЗ), то ми можемо використовувати
             * тільки влативості власне об'єктів Room
             * 
             * Саме тому ми спочатку групуємо список rooms по властивості Floor,
             * після чого форматуємо вибірку під анонімний тип з допомогою Select,
             * створивши в ньому властивості Floor, яка буде рівна номеру поверху
             * а також створивши властивість RoomCount, яка буде рівна кількості 
             * самих об'єктів Room з ключем, що відповідає поверху.
             * 
             * Маючи відформатовану вибірку з поверхами та кількістю кімнат ми 
             * викликаємо на ній метод OrderBy, вказуючи, що впорядкування повинно відбуватись
             * за кількістю кімнат на поверсі в порядку від найменшої кількості до 
             * найбільшої (за-замовчуванням)
             */

            var floorsWithRoomCount = rooms
                .GroupBy(r => r.Floor)
                .Select(g => new { Floor = g.Key, RoomCount = g.Count() })
                .OrderBy(f => f.RoomCount);

            /*
             * Далі нам потрібно визначити і записати в змінну найменшу кількість
             * кімнат,  яка в принципі існує в готелі, для цього нам достатньо вибрати
             * найперший об'єкт з вибірки (так як вона і так попередньо впорядкована)
             * і його властивість RoomCount записати в змінну
             */

            int minRoomCount = floorsWithRoomCount.First().RoomCount;

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
                .Where(f => f.RoomCount == minRoomCount)
                .Select(f => f.Floor);

            Console.WriteLine($"Floors with small amount rooms: {string.Join(", ", floors)}");
        }


        public void CalculatePaymentForCurrentVisitors(List<Visitor> visitors, List<Room> rooms)
        {
            /*
             * За умовами завдання нам потрібно вивести кожному гостю готеля, який ще не
             * виселився, суму його заборгованості по теперішній день
             * 
             * Для того, щоб визначити зі списку гостей саме тих, які ще не виселились,
             * нам потрібно перебрати список visitors таким чином, щоб дата їх виселення
             * була більшою за теперішню дату. Таким чином нам вдасться відсіяти тих, хто 
             * вже виселився.
             * 
             * Для цього ми створюємо вибірку currentVisitors, де задаємо відповідну умову
             * для властивості EvictionTime
             */

            DateTime currentDate = DateTime.Now;
            var currentVisitors = visitors.Where(v => v.EvictionTime > currentDate);

            /*
             * Маючи потрібну вибірку ми робимо по ній прохід циклом, ціллю якого
             * є визначення суми заборгованості, яку ми можемо дізнатись тільки в випадку,
             * якщо на основі властивості RoomNumber зможемо витягнути з списку rooms
             * відповідну кімнату і записати її в змінну room.
             * 
             * Після цього ми дізнаємось залишок днів до виселення з допомогою різниці
             * теперішньої дати і дати розрахунку і записавши це все в змінну dayStayed
             * 
             * Для того, щоб дізнатись суму заборгованості ми просто перемножуємо
             * вартість проживання в кімнаті на кількість днів проживання за які треба
             * заплатити і записуємо значення в змінну totalPayment
             * 
             * Знаючи ці дані - виводимо їх в консоль. Кожен прохід циклом буде виводити
             * дані в консоль відповіно до кількості гостей в вибірці
             * 
             */

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
