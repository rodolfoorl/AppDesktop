using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Desktop.GrupoBW.Util
{
    public static class Util
    {
        public static double PegarNumeros(string valor)
        {
            valor = valor.Replace('.', ',');

            if (string.IsNullOrEmpty(valor))
                valor = "0";

            Regex regex = new Regex(",|[0-9]");
            StringBuilder numeros = new StringBuilder();

            foreach (Match m in regex.Matches(valor))
            {
                numeros.Append(m.Value);
            }

            return double.Parse(numeros.ToString());
        }
    }
}
