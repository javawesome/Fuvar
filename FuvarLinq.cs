using System;       // Console
using System.IO;    // StreamReader
using System.Text;  // Encoding
using System.Collections.Generic; // List<>, Dictionary<>
using System.Linq;  // from where group by orderby select

namespace Fuvar_linq
{
    class Fuvar
    {
        public int taxi_id { get; set; }
        public string indulas { get; set; }
        public int idotartam { get; set; }
        public float tavolsag { get; set; }
        public float viteldij { get; set; }
        public float borravalo { get; set; }
        public string fizetes_modja { get; set; }

        public Fuvar(string sor)
        {
            //var s = sor.Trim().Replace(',', '.').Split(';');
            var s = sor.Trim().Split(';');
            this.taxi_id = int.Parse(s[0]);
            this.indulas = s[1];
            this.idotartam = int.Parse(s[2]);
            this.tavolsag = float.Parse(s[3]);
            this.viteldij = float.Parse(s[4]);
            this.borravalo = float.Parse(s[5]);
            this.fizetes_modja = s[6];
        }
    }
    class FuvarLinq
    {
        static void Main(string[] args)
        {
            // 2. feladat 
            var f = new StreamReader("fuvar.csv");
            var elsosor = f.ReadLine();
            var lista = new List<Fuvar>();
            while (!f.EndOfStream)
            {
                lista.Add(new Fuvar(f.ReadLine()));
            }
            f.Close();

            // 3. feladat: {} fuvar
            Console.WriteLine($"3. feladat: {lista.Count} fuvar");

            // 4. feladat: a 6185 -ös fuvarjainak száma és bevétele
            var bevetelek = (
                from sor in lista
                where sor.taxi_id == 6185
                select (sor.viteldij + sor.borravalo)
                );

            Console.WriteLine($"4. feladat: {bevetelek.Count()} fuvar alatt: {bevetelek.Sum()}$");

            // 5. feladat: Fizetési statisztika
            var query = (
                from sor in lista
                group sor by sor.fizetes_modja
                );

            Console.WriteLine($"5. feladat:");
            foreach (var q in query)
            {
                Console.WriteLine($"        {q.Key}: {q.Count()} fuvar");
            }

            // 6. feladat Összes km-ek száma
            var tavolsagok = (
                from sor in lista
                select sor.tavolsag * 1.6
                );
            Console.WriteLine($"6. feladat: {tavolsagok.Sum():.##}km");

            // 7. feladat: Leghosszabb idejű fuvar:
            var leghosszab_fuvar = (
                from sor in lista
                orderby sor.idotartam
                select sor
                ).Last();
            Console.WriteLine($"7. feladat:");
            Console.WriteLine($"        Fuvar hossza: {leghosszab_fuvar.idotartam} másodperc:");
            Console.WriteLine($"        Taxi azonosító: {leghosszab_fuvar.taxi_id} ");
            Console.WriteLine($"        Megtett távolság: {leghosszab_fuvar.tavolsag:.#} km");
            Console.WriteLine($"        Viteldíj: {leghosszab_fuvar.viteldij}$");

            // 8. feladat hibak.txt létrehozása
            var hibak = (
                from sor in lista
                where (sor.idotartam > 0) & (sor.viteldij > 0) & (sor.tavolsag == 0)
                orderby sor.indulas
                select sor
            );
            var f_hibak = new StreamWriter("Hibak.txt");
            f_hibak.WriteLine(elsosor);
            foreach (var i in hibak)
            {
                var sor = $"{i.taxi_id};{i.indulas};{i.idotartam};{i.tavolsag};{i.viteldij};{i.borravalo};{i.fizetes_modja}";
                f_hibak.WriteLine(sor.Replace('.', ','));
            }
            f_hibak.Close();
            //-------------------------------------     
        }
    }
}
