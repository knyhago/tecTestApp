using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entities;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
   
     public DataContext(DbContextOptions<DataContext> options) : base(options) { }

     public DataContext(){
        
     }

     public DbSet<User> Users { get; set; }

    public DbSet<Log> UserLogs { get; set; }


    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    //     => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        { 
        model.Entity<User>()
        .Property(u => u.DateOfBirth)
        .HasConversion(
            d => d.ToDateTime(TimeOnly.MinValue),  // store as DateTime
            d => DateOnly.FromDateTime(d)); 
        
        model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true,DateOfBirth=new DateOnly (1995,6,12 )},
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true,DateOfBirth=new DateOnly (1995,11,21 ) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false,DateOfBirth=new DateOnly (1998,4,15 ) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true ,DateOfBirth=new DateOnly (2005,3,17 )},
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true,DateOfBirth=new DateOnly (1980,5,20 ) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true,DateOfBirth=new DateOnly (2000,12,18 ) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false,DateOfBirth=new DateOnly (2002,2,27  ) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false,DateOfBirth=new DateOnly (2012,12,13 ) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false,DateOfBirth=new DateOnly (2008,12,29 ) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true,DateOfBirth=new DateOnly (1999,2,17 ) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true,DateOfBirth=new DateOnly (1997,1,6 ) },
        });
        }

   
    public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
    {
    return await Set<TEntity>().ToListAsync(); // materializes IQueryable
    }


    public async Task Create<TEntity>(TEntity entity) where TEntity : class
    {
        await base.AddAsync(entity);
        await SaveChangesAsync();
    }

    public new async Task Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }

    
}
