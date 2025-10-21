using Domain.Cities;
using Domain.Countries;
using Domain.Feedbacks;
using Domain.Followers;
using Domain.PlacePhotos;
using Domain.Places;
using Domain.Roles;
using Domain.Travels;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Follower> Followers { get; set; } = null!;
    public DbSet<Place> Places { get; set; } = null!;
    public DbSet<PlacePhoto> PlacePhotos { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Travel> Travels { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TravelPlace> TravelPlaces { get; set; } = null!;
    public DbSet<UserTravel> UserTravels { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}