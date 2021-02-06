﻿using FootballersDirectory.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersDirectory.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Team> Teams { get; set; } 
        public DbSet<Footballer> Footballers { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public async void AddTeam(Team team)
        {
            await Teams.AddAsync(team);
            await SaveChangesAsync();
        }
    }
}
