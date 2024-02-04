﻿using HrAppSimple.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace HrAppSimple.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
            
        }

        public DbSet<Angajat> Angajati{ get; set; }
        public DbSet<AngajatProiect> AngajatiProiecte { get; set; }
        public DbSet<Departament> Departamente { get; set; }
        public DbSet<DetaliiAngajat> DetaliiAngajati { get; set; }
        public DbSet<Locatie> Locatii { get; set; }
        public DbSet<Proiect> Proiecte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Angajat>()
                .HasKey(a => a.Matricula);
            modelBuilder.Entity<Departament>()
                .HasKey(d => d.CodDepartament);
            modelBuilder.Entity<DetaliiAngajat>()
                .HasKey(da => da.Matricula);
            modelBuilder.Entity<Locatie>()
                .HasKey(d => d.CodLocatie);
            modelBuilder.Entity<Proiect>()
                .HasKey(da => da.CodProiect);
               
            //many to many
            modelBuilder.Entity<AngajatProiect>()
                .HasKey(ap => new { ap.Matricula, ap.CodProiect });

            modelBuilder.Entity<AngajatProiect>()
                .HasOne(a => a.Angajat)
                .WithMany(ap => ap.AngajatiProiecte)
                .HasForeignKey(p => p.Matricula);

            modelBuilder.Entity<AngajatProiect>()
                .HasOne(a => a.Proiect)
                .WithMany(ap => ap.AngajatiProiecte)
                .HasForeignKey(p => p.CodProiect);

            //one to one
            modelBuilder.Entity<Angajat>()
                .HasOne(e => e.DetaliiAngajat)
                .WithOne(e => e.Angajat)
                .HasForeignKey<DetaliiAngajat>(e => e.MatriculaAng)
                .IsRequired();

            modelBuilder.Entity<DetaliiAngajat>()
                .HasOne(e => e.Angajat)
                .WithOne(e => e.DetaliiAngajat)
                .HasForeignKey<DetaliiAngajat>(e => e.MatriculaAng)
                .IsRequired();
        }
    }
}
