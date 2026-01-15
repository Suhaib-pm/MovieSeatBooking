using System;

namespace MovieSeatBooking.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public SeatStatus Status { get; set; }
        public DateTime? HoldTill { get; set; }
    }
}
