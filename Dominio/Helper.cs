using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dominio
{
    public static class Helper
    {
        public static void cargarImagen(string imagen, PictureBox pbx)
        {
            try
            {
                pbx.Load(imagen);
            }
            catch (Exception)
            {
                pbx.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg"); ;
            }
        }
        public static List<Articulo> mayorPrecio(List<Articulo> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                for (int j = 0; j < lista.Count - i - 1; j++)
                {
                    if (lista[j].Precio < lista[j + 1].Precio)
                    {
                        Articulo aux = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = aux;
                    }
                }
            }

            return lista;
        }
        public static List<Articulo> menorPrecio(List<Articulo> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                for (int j = 0; j < lista.Count - i - 1; j++)
                {
                    if (lista[j].Precio > lista[j + 1].Precio)
                    {
                        Articulo aux = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = aux;
                    }
                }
            }
            return lista;
        }
 
        
    }
}
