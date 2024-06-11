using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;
using System.IO;

namespace Presentación
{
    public partial class VentanaPrincipal : Form
    {
        private Articulo articulo = null;
        private Articulo seleccionado = null;
        private List<Articulo> listaArticulos;
        private List<Articulo> listaFiltrada = null;
        private OpenFileDialog archivo = null;
        public VentanaPrincipal()
        {
            InitializeComponent();
        }


        // CARGA DE FORM
        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCategorias.Items.Add("Celulares");
            cbxCategorias.Items.Add("Televisores");
            cbxCategorias.Items.Add("Media");
            cbxCategorias.Items.Add("Audio");
            cbxMarcas.Items.Add("Samsung");
            cbxMarcas.Items.Add("Motorola");
            cbxMarcas.Items.Add("Apple");
            cbxMarcas.Items.Add("Sony");
            cbxMarcas.Items.Add("Huawei");
            cbxOrdenarPor.Items.Add("Mayor Precio");
            cbxOrdenarPor.Items.Add("Menor Precio");


        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;
                cbxMarca.DataSource = marcaNegocio.listar();
                cbxCategoria.DataSource = categoriaNegocio.listar();
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
   
        // BUSCADOR Y FILTROS
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                string marca = null, categoria = null;

                if (Validar.Null(cbxCategorias.SelectedItem))
                    categoria = cbxCategorias.SelectedItem.ToString();
                if (Validar.Null(cbxMarcas.SelectedItem))
                    marca = cbxMarcas.SelectedItem.ToString();

                listaFiltrada = negocio.filtrar(categoria, marca);

                if (txtBuscar.Text != "")
                    listaFiltrada = listaFiltrada.FindAll(x => x.Nombre.ToUpper().Contains(txtBuscar.Text.ToUpper()));

                if (Validar.Null(cbxOrdenarPor.SelectedItem))
                {
                    if (cbxOrdenarPor.SelectedItem.ToString() == "Mayor Precio")
                        listaFiltrada = Helper.mayorPrecio(listaFiltrada);
                    else
                        listaFiltrada = Helper.menorPrecio(listaFiltrada);
                }

                dgvArticulos.DataSource = listaFiltrada;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            if (listaFiltrada != null)
            {
                cbxCategorias.SelectedItem = null;
                cbxMarcas.SelectedItem = null;
                cbxOrdenarPor.SelectedItem = null;
                txtBuscar.Text = null;
                dgvArticulos.DataSource = listaArticulos;
            }


        }


        // BOTONES
        private void btnVerArticulo_Click(object sender, EventArgs e)
        {
            if (!(Validar.Null(dgvArticulos.CurrentRow)))
                return;

            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            VentanaDetalle ventana = new VentanaDetalle(seleccionado);

            ventana.ShowDialog();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                // VALIDAMOS CAJAS VACIAS (ESTAS 3 SON OBLIGATORIAS)
                if(!(Validar.cajaVacia(txtCodigo.Text) && Validar.cajaVacia(txtPrecio.Text) && Validar.cajaVacia(txtNombre.Text)))
                    return; 

                if (articulo == null)
                    articulo = new Articulo();

                articulo.Nombre = txtNombre.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.Marca = (Marca)cbxMarca.SelectedItem;
                articulo.Categoria = (Categoria)cbxCategoria.SelectedItem;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagen.Text;


                // VALIDAMOS EL CODIGO (ADMITE SOLO 3 CARACTERES)
                if (Validar.Codigo(txtCodigo.Text))
                {
                    articulo.Codigo = txtCodigo.Text;

                    if (archivo != null && !(txtImagen.Text.ToUpper().Contains("HTTP")) && File.Exists(archivo.FileName))
                    {
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["articulos-imagen"] + archivo.SafeFileName);
                        articulo.ImagenUrl = ConfigurationManager.AppSettings["articulos-imagen"] + archivo.SafeFileName;
                    }
                    else
                        articulo.ImagenUrl = txtImagen.Text;

                    // MODIFICO O AGREGO
                    if (articulo.Id != 0)
                    {
                        negocio.modificar(articulo);
                        MessageBox.Show("¡Modificado Exitosamente!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarCajas();
                        cargar();
                    }
                    else
                    {
                        negocio.agregar(articulo);
                        MessageBox.Show("¡Agregado Exitosamente!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarCajas();
                        cargar();
                    }
                }
                else
                    MessageBox.Show("Ingrese un codigo de 3 digitos.", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese un precio valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("Ingresa otra imagen.", "Error Imagen Existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!(Validar.Null(dgvArticulos.CurrentRow)))
                return;

            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                DialogResult resultado = MessageBox.Show("¿Desea eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);

                    if(!(seleccionado.ImagenUrl.ToUpper().Contains("HTTP")) && !(string.IsNullOrEmpty(seleccionado.ImagenUrl)))
                        File.Delete(seleccionado.ImagenUrl);

                    limpiarCajas();
                    cargar();
                    
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (!(Validar.Null(dgvArticulos.CurrentRow)))
                return;
            
            articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            try
            {
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";

             
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtPrecio.Text = articulo.Precio.ToString();
                txtImagen.Text = articulo.ImagenUrl;
                Helper.cargarImagen(articulo.ImagenUrl, pbxImagen);
                txtDescripcion.Text = articulo.Descripcion;
                cbxMarca.SelectedValue = articulo.Marca.Id;
                cbxCategoria.SelectedValue = articulo.Categoria.Id;

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";

            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = archivo.FileName;
                Helper.cargarImagen(archivo.FileName, pbxImagen);

            } 
        }


        // OTROS
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["Codigo"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
        }
        private void limpiarCajas()
        {
            if (articulo != null)
            {
                seleccionado = null;
                articulo = null;
                txtCodigo.Text = null;
                txtNombre.Text = null;
                txtPrecio.Text = null;
                txtImagen.Text = null;
                pbxImagen.Image = null;
                txtDescripcion.Text = null;
            }
        }
        private void txtImagen_Leave(object sender, EventArgs e)
        {
            Helper.cargarImagen(txtImagen.Text, pbxImagen);
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            limpiarCajas();
        }

    }   
}
