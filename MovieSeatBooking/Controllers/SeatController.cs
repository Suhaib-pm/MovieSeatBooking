using Microsoft.AspNetCore.Mvc;
using MovieSeatBooking.Data;
using MovieSeatBooking.Models;

namespace MovieSeatBooking.Controllers
{
    [ApiController]
    [Route("api/seats")]
    public class SeatController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeatController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("count/{showId}")]
        public IActionResult GetSeatCount(int showId)
        {
            var available = _context.Seats.Count(s => s.ShowId == showId && s.Status == SeatStatus.Available);
            var held = _context.Seats.Count(s => s.ShowId == showId && s.Status == SeatStatus.Held);
            var booked = _context.Seats.Count(s => s.ShowId == showId && s.Status == SeatStatus.Booked);

            return Ok(new { available, held, booked });
        }

        
        [HttpPost("hold")]
        public IActionResult HoldSeats(int showId, List<int> seatIds)
        {
            var seats = _context.Seats
                .Where(s => seatIds.Contains(s.Id) && s.ShowId == showId)
                .ToList();

            if (seats.Any(s => s.Status != SeatStatus.Available))
                return BadRequest("Seat already taken");

            foreach (var seat in seats)
            {
                seat.Status = SeatStatus.Held;
                seat.HoldTill = DateTime.Now.AddMinutes(5);
            }

            _context.SaveChanges();
            return Ok("Seats held successfully");
        }

       
        [HttpPost("book")]
        public IActionResult BookSeats(int showId)
        {
            var seats = _context.Seats
                .Where(s => s.ShowId == showId && s.Status == SeatStatus.Held)
                .ToList();

            foreach (var seat in seats)
            {
                seat.Status = SeatStatus.Booked;
                seat.HoldTill = null;
            }

            _context.SaveChanges();
            return Ok("Booking successful");
        }

        
        [HttpPost("release")]
        public IActionResult ReleaseSeats()
        {
            var expiredSeats = _context.Seats
                .Where(s => s.Status == SeatStatus.Held && s.HoldTill < DateTime.Now)
                .ToList();

            foreach (var seat in expiredSeats)
            {
                seat.Status = SeatStatus.Available;
                seat.HoldTill = null;
            }

            _context.SaveChanges();
            return Ok("Expired seats released");
        }
    }
}
