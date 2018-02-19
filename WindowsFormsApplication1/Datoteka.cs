using System;
using System.Collections.Generic;
using System.IO;

namespace NASP
{
    public class Datoteka
    {
        private readonly String _lokacija = "";
        public Datoteka(String lokacija)
        {
            if (lokacija != null)
            {
                _lokacija = lokacija;
            }
        }

        public List<int> Citaj()
        {
            var procitaniBr = new List<int>();
            if (_lokacija.Length == 0)
            {
                return procitaniBr;
            }
            var datoteka = new StreamReader(_lokacija);

            try
            {
                var procitaniTekst = datoteka.ReadToEnd();
                var brojevi = procitaniTekst.Split(' ');


                foreach (var znak in brojevi)
                {
                    var znamenke = znak.Trim();
                    try
                    {
                        int broj;
                        Int32.TryParse(znamenke, out broj);
                        procitaniBr.Add(broj);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }

            finally
            {
                datoteka.Close();
            }

            return procitaniBr;
        }
    }
}
