using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public static class Validar
    {
        public static bool Codigo(string codigo)
        {
            if(codigo.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool soloTexto(string texto)
        {
            foreach (char caracter in texto)
            {
                if (char.IsNumber(caracter))
                    return false;
            }

            return true;

        }
        public static bool soloNumeros (string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }

            return true;
        }
        public static bool cajaVacia (string caja)
        {
            if(string.IsNullOrEmpty(caja))
                return false;
            return true;
         
        }
        public static bool Null(object seleccionado)
        {
            if(seleccionado == null)
                return false;
            return true;
        }
    }
}
