using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        DB datos = new DB();
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            try
            {
                
                datos.setearConsulta("SELECT Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, A.IdMarca, A.IdCategoria, A.Id FROM ARTICUlOS A, MARCAS M, CATEGORIAS C WHERE M.Id = A.IdMarca AND C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    if (!(datos.Lector["Descripcion"]  is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                   

                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];

                    lista.Add(aux);

                }
                return lista;
            }   
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            
        }       
        
        public List<Articulo> filtrar(string categoria, string marca)
        {
            List<Articulo> lista = new List<Articulo>();

            try
            {
                string consulta = "SELECT Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, A.IdMarca, A.IdCategoria, A.Id FROM ARTICUlOS A, MARCAS M, CATEGORIAS C WHERE M.Id = A.IdMarca AND C.Id = A.IdCategoria";
                
                if(categoria != null)
                {
                    if (categoria == "Celulares" || categoria == "Televisores" || categoria == "Media" || categoria == "Audio")
                    {
                        if (marca != null)
                        {
                            if (marca == "Samsung" || marca == "Motorola" || marca == "Apple" || marca == "Sony" || marca == "Huawei")
                            {
                                consulta += " AND C.Descripcion = '" + categoria + "' AND M.Descripcion = '" + marca + "'";
                            }
                               
                        }
                        else
                        {
                            consulta += " AND C.Descripcion = '" + categoria + "'";
                        }
                    }
                }
                else if(marca != null)
                {
                    consulta += " AND M.Descripcion = '" + marca + "'";
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Precio = (decimal)datos.Lector["Precio"];


                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];

                    lista.Add(aux);

                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Articulo nuevo)
        {
            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values(@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                datos.setearParametro("codigo", nuevo.Codigo);
                datos.setearParametro("nombre", nuevo.Nombre);
                datos.setearParametro("descripcion", nuevo.Descripcion);
                datos.setearParametro("idMarca", nuevo.Marca.Id);
                datos.setearParametro("idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("imagenUrl", nuevo.ImagenUrl);
                datos.setearParametro("precio", nuevo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            try
            {
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            { 
                datos.cerrarConexion();
            }
        }
        public void modificar(Articulo articulo)
        {
            try
            {
                datos.setearConsulta("UPDATE ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idmarca, IdCategoria = @idcategoria, ImagenUrl = @imagen, Precio = @precio WHERE Id = @id");
                datos.setearParametro("id", articulo.Id);
                datos.setearParametro("nombre", articulo.Nombre);
                datos.setearParametro("codigo", articulo.Codigo);
                datos.setearParametro("descripcion", articulo.Descripcion);
                datos.setearParametro("idmarca", articulo.Marca.Id);
                datos.setearParametro("idcategoria", articulo.Categoria.Id);
                datos.setearParametro("imagen", articulo.ImagenUrl);
                datos.setearParametro("precio", articulo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
