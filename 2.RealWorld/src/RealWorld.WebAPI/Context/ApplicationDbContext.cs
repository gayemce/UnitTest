using Microsoft.EntityFrameworkCore;
using RealWorld.WebAPI.Models;

namespace RealWorld.WebAPI.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}
