using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetsHeroe.iOS;
using Foundation;
using UIKit;
using System.Data;

[assembly: Xamarin.Forms.Dependency(typeof(IOSService))]
namespace PetsHeroe.iOS
{
    class IOSService : IIOS
    {
        public DataTable CAM_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Ciudad_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Codigo_Valida { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Estado_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MarcaProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaColor_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaEstatus_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaTipo_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Pais_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Producto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Servicio_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable TipoAsociado_Busca { get; set; }
        public DataTable TipoProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable ValidaUsuario { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void getCAM_busca()
        {
            throw new NotImplementedException();
        }

        public void getCiudad_Buscs()
        {
            throw new NotImplementedException();
        }

        public void getCodigo_Valida()
        {
            throw new NotImplementedException();
        }

        public void getEstado_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMarcaProducto_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMascotaColor_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMascotaEstatus_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMascotaTipo_Busca()
        {
            throw new NotImplementedException();
        }

        public void getPais_Busca()
        {
            throw new NotImplementedException();
        }

        public void getProducto_Busca()
        {
            throw new NotImplementedException();
        }

        public void getServicio_Busca()
        {
            throw new NotImplementedException();
        }

        public void getTipoAsociado_Busca()
        {

            throw new NotImplementedException();
        }

        public void getTipoProducto_Busca()
        {
            throw new NotImplementedException();
        }

        public void getValidaUsuario()
        {
            throw new NotImplementedException();
        }
    }
}