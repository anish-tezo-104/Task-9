using EMS.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.DB.Context;
public class EMSContext : DbContext
{

    public EMSContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<Employee> Employee { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<Project> Project { get; set; }
    public DbSet<Location> Location { get; set; }

    
}