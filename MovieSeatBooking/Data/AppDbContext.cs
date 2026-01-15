using Microsoft.EntityFrameworkCore;
using MovieSeatBooking.Models;
using System.Collections.Generic;

namespace MovieSeatBooking.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Seat> Seats { get; set; }
    }
}
