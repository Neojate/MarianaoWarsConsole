using MarianaoWars.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarianaoWarsConsole.Data
{
    public class MarianaoWarsContext : DbContext
    {

        public DbSet<Enrollment> Enrollment { get; set; }

        public DbSet<Institute> Institute { get; set; }

        public DbSet<Computer> Computer { get; set; }

        public DbSet<Resource> Resource { get; set; }

        public DbSet<Software> Software { get; set; }

        public DbSet<Talent> Talent { get; set; }

        public DbSet<AttackScript> AttackScript { get; set; }

        public DbSet<DefenseScript> DefenseScript { get; set; }

        public DbSet<SystemResource> SystemResource { get; set; }

        public DbSet<SystemSoftware> SystemSoftware { get; set; }

        public DbSet<BuildOrder> BuildOrder { get; set; }

        public DbSet<HackOrder> HackOrder { get; set; }

        public DbSet<Message> Message { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=127.0.0.1;port=3306; database=marianaowars;user=root;password=");
        }

    }
}
