using Microsoft.EntityFrameworkCore;
using SportSistem.Models.Entities;

namespace SportSistem.Data;

public class SportDbContext: DbContext
{
    public SportDbContext(DbContextOptions<SportDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Space> Spaces { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}