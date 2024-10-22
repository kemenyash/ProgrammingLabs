using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore
{
    [Table("durations")]
    public class Duration
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("arrival")]
        public DateTime Arrival {  get; set; }
        [Column("eviction")]
        public DateTime Eviction { get; set; }
        [Column("guest_id")]
        public int GuestId { get; set; }
        [Column("room_id")]
        public int RoomId { get; set; }

        public Room Room { get; set; }
        public Guest Guest { get; set; }
    }
}
