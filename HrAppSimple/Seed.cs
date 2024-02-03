using HrAppSimple.Data;
using HrAppSimple.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HrAppSimple
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Angajat.Any() && !dataContext.AngajatProiect.Any() && !dataContext.Departament.Any()
                && !dataContext.DetaliiAngajat.Any() && !dataContext.Locatie.Any() && !dataContext.Proiect.Any())
            {
                var departamente = new List<Departament>
                {
                    new Departament { CodDepartament = 1, Denumire = "IT" },
                    new Departament { CodDepartament = 2, Denumire = "Vânzări" }
                };

                dataContext.Departament.AddRange(departamente);
                dataContext.SaveChanges();

                var locatii = new List<Locatie>
                {
                    new Locatie { CodLocatie = 1, Oras = "Bucuresti", Tara = "Romania"},
                    new Locatie { CodLocatie = 2, Oras = "Cluj-Napoca", Tara = "Romania" }
                };

                dataContext.Locatie.AddRange(locatii);
                dataContext.SaveChanges();

                var proiecte = new List<Proiect>
                {
                    new Proiect { CodProiect = 1, Denumire = "Proiect1", DataPredare = DateTime.Now },
                    new Proiect { CodProiect = 2, Denumire = "Proiect2", DataPredare = DateTime.Now.AddDays(10) }
                };

                dataContext.Proiect.AddRange(proiecte);
                dataContext.SaveChanges();

                var angajati = new List<Angajat>
                {
                    new Angajat
                    {
                        Matricula = 1,
                        CodDepartament = 1,
                        Departament = departamente[0],
                        DetaliiAngajat = new DetaliiAngajat { Matricula = 1, Nume = "Nume1", Prenume = "Prenume1", Email = "email1@example.com", DataNastere = DateTime.Now, DataAngajare = DateTime.Now },
                        AngajatProiect = new List<AngajatProiect> { new AngajatProiect { Matricula = 1, CodProiect = 1 } }
                    },
                    
                };

                dataContext.Angajat.AddRange(angajati);
                dataContext.SaveChanges();
            }
        }
    }
}
