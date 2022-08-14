using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;

namespace ParkyAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<NationalPark> NationalParks => Set<NationalPark>();
    //public DbSet<NationalPark>? NationalParks { get; set; }

}
