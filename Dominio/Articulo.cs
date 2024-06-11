using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        public string Codigo { get; set; }
        [DisplayName("Modelo")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }
        public int Id {  get; set; }
    }
}
