using System;
using System.Collections.Generic;
using System.IO;

namespace NASP
{
    public class FileReader
    {
        private readonly String _path = "";
        public FileReader(String path)
        {
            if (path != null)
            {
                _path = path;
            }
        }

        public List<int> Read()
        {
            var ints = new List<int>();
            if (_path.Length == 0)
            {
                return ints;
            }
            var streamReader = new StreamReader(_path);

            try
            {
                var procitaniTekst = streamReader.ReadToEnd();
                var numbers = procitaniTekst.Split(' ');


                foreach (var _char in numbers)
                {
                    var number = _char.Trim();
                    try
                    {
                        int broj;
                        Int32.TryParse(number, out broj);
                        ints.Add(broj);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }

            finally
            {
                streamReader.Close();
            }

            return ints;
        }
    }
}
