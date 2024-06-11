using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;

namespace Presentación
{
    public partial class VentanaDetalle : Form
    {
        Articulo articulo = null;
        public VentanaDetalle()
        {
            InitializeComponent();
        }
        public VentanaDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }
        private void VentanaDetalle_Load(object sender, EventArgs e)
        {
            lblCodigo.Text = articulo.Codigo;
            lblModelo.Text = articulo.Nombre;
            lblPrecio.Text += articulo.Precio.ToString();
            Helper.cargarImagen(articulo.ImagenUrl, pbxImagen);
            txtDescripcion.Text = articulo.Descripcion;
            lblMarca.Text = articulo.Marca.Descripcion.ToString();
            lblCategoria.Text = articulo.Categoria.Descripcion.ToString();
        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
