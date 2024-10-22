using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("room_number")]
        public int RoomNumber { get; set; }
        [Column("guest_count")]
        public int GuestsCount { get; set; }
        [Column("cost_per_night")]
        public decimal CostPerNight { get; set; }
        [Column("settlement_time")]
        public TimeSpan SettlementTime { get; set; }
        [Column("eviction_time")]
        public TimeSpan EvictionTime { get; set; }
        [Column("floor_id")]
        public int FloorId { get; set; }

        public Floor Floor { get; set; }
    }
}
