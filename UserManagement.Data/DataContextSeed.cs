using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data;


    public static class DataContextSeed
    {
        public static void Seed(DataContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>
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

                context.SaveChanges();
            }
        }
    }


