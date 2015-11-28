﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataGenerator
{
    class Oddzial : Encja
    {
        private string NrOddzialu;
        private string TelefonKontaktowy;
        private string FkAdresUlica;
        private string FkAdresKodPocztowy;
        private int LiczbaOddzialow;
        private string LiczbaFirmKonkurencyjnych;
        private string NumerOddzialuNadrzednego;
        
        public Oddzial(int n)
        {
            LiczbaOddzialow = n;
        }

        public override void Randomize()
        {
            System.IO.StreamWriter sw;
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
                sw = new System.IO.StreamWriter(Program.Path + "Oddzial.sql");
            else
                sw = new System.IO.StreamWriter(Program.Path + "Oddzial2.sql");


            Random rnd = new Random();

            string line = String.Empty;

            for (int i = 0; i < LiczbaOddzialow; i++)
            {
                int im;
                do
                {
                    im = rnd.Next(0, Program.LiczbaAdresow);
                }
                while (Program.zajeteAdresy[im] == true);

                Program.zajeteAdresy[im] = true;

                FkAdresUlica = Program.Adresy[im].Ulica;
                FkAdresKodPocztowy = Program.Adresy[im].KodPocztowy;

                int cyfra = 0;
                for(int j = 0; j < 9; j++)
                {
                    if(j == 0)
                    {
                        cyfra = rnd.Next(1, 10);
                    }
                    else
                    {
                        cyfra = rnd.Next(0, 10);
                    }

                    TelefonKontaktowy += cyfra.ToString();
                }
                    LiczbaFirmKonkurencyjnych = rnd.Next(0, 20).ToString();
                    NumerOddzialuNadrzednego = rnd.Next(1, Program.LiczbaOddzialow).ToString();
                    sw.WriteLine("insert into Oddzial values(" + "'" + TelefonKontaktowy + "'" + "," + LiczbaFirmKonkurencyjnych + "," + "'" + NumerOddzialuNadrzednego + "'" + "," + "'" + FkAdresUlica + "'" + "," + "'" + FkAdresKodPocztowy + "')");
                TelefonKontaktowy = String.Empty;
                sw.Flush();
            }

            sw.Close();
        }

        public override void Create(System.IO.StreamWriter f)
        {
            f.WriteLine("CREATE TABLE Oddzial");
            f.WriteLine("(");

            f.WriteLine("\tNrOddzialu INTEGER IDENTITY(1,1) PRIMARY KEY,");
            f.WriteLine("\tTelefonKontaktowy varchar(12),");
            f.WriteLine("\tLiczbaFirmKonkurencyjnych int,");
            f.WriteLine("\tNumerOddzialuNadrzednego int,");
            f.WriteLine("\tFkAdresUlica varchar(60),");
            f.WriteLine("\tFkAdresKodPocztowy varchar(8),");
            f.WriteLine("\tFOREIGN KEY(FkAdresUlica, FkAdresKodPocztowy) REFERENCES Adres");

            f.WriteLine(")");
        }

        public override void Insert(StreamWriter file)
        {
            if (Program.PunktCzasowy == Program.TimePoint.FirstTimePoint)
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Oddzial.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            else
            {
                string[] p = System.IO.File.ReadAllLines(Program.Path + "Oddzial2.sql");

                foreach (string line in p)
                {
                    file.WriteLine(line);
                }
                file.WriteLine();
            }
            file.WriteLine();
        }

        public override void Update(System.IO.StreamWriter file)
        {
            Random rnd = new Random();
            for(int i = 0; i < 25; i++)
            {
                int nrOddzialu = rnd.Next(1, Program.LiczbaOddzialow + 1);

                int im;
                do
                {
                    im = rnd.Next(0, Program.LiczbaAdresow);
                }
                while (Program.zajeteAdresy[im] == true);

                Program.zajeteAdresy[im] = true;

                file.WriteLine("update Oddzial set \"FkAdresUlica\" = '{0}', \"FkAdresKodPocztowy\" = '{1}' where \"NrOddzialu\" = {2};",
                    Program.Adresy[im].Ulica.ToString(), Program.Adresy[im].KodPocztowy.ToString(), nrOddzialu.ToString());
            }
        }
    }
}