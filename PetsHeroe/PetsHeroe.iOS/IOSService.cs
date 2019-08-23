using System;
using PetsHeroe.iOS;
using System.Data;
using PetsHeroe.iOS.mx.com.petshero;

[assembly: Xamarin.Forms.Dependency(typeof(IOSService))]
namespace PetsHeroe.iOS
{
    class IOSService : IIOS
    {
        public DataTable CAM_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Ciudad_Busca { get; set; }
        public DataTable Codigo_Valida { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Estado_Busca { get; set; }
        public DataTable MarcaProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaColor_Busca { get; set; }
        public DataTable MascotaEstatus_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaTipo_Busca { get; set; }
        public DataTable Pais_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Producto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Servicio_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable TipoAsociado_Busca { get; set; }
        public DataTable TipoProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable ValidaUsuario { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaRaza_Busca { get; set; }

        //Variables out
        public int IDUsuario { get; set; }
        public int IDMiembro { get; set; }
        public int IDAsociado { get; set; }
        public string nombre { get; set; }
        //

        wsPetsApp wsPets = new wsPetsApp();
        AuthHeader auth = new AuthHeader()
        {
            Usuario = "appcelmypets2019",
            Password = "RRW7G0ZiF4D1bUasqazmTg",
            IDUsuario = 0,
            IPAddress = "0"
        };


        public void getCAM_busca()
        {
            throw new NotImplementedException();
        }

        public void getCiudad_Busca(int IDEstado)
        {
            wsPets.AuthHeaderValue = auth;
            Ciudad_Busca = wsPets.Ciudad_Busca(IDEstado);
        }

        public void getCodigo_Valida()
        {
            throw new NotImplementedException();
        }

        public void getEstado_Busca()
        {
            wsPets.AuthHeaderValue = auth;
            Estado_Busca = wsPets.Estado_Busca(1);
        }

        public void getMarcaProducto_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMascotaColor_Busca(int IDTipo)
        {
            wsPets.AuthHeaderValue = auth;
            MascotaColor_Busca = wsPets.MascotaColor_Busca(IDTipo);
        }

        public void getMascotaEstatus_Busca()
        {
            throw new NotImplementedException();
        }

        public void getMascotaTipo_Busca()
        {
            wsPets.AuthHeaderValue = auth;
            MascotaTipo_Busca = wsPets.MascotaTipo_Busca();
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
            wsPets.AuthHeaderValue = auth;
            TipoAsociado_Busca = wsPets.TipoAsociado_Busca().Copy();
        }

        public void getTipoProducto_Busca()
        {
            throw new NotImplementedException();
        }

        public bool getValidaUsuario(string user, string pass)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                bool result = wsPets.ValidaUsuario(user, pass, out int IDUsuario, out int IDMiembro, out int IDAsociado, out string nombre);
                this.IDUsuario = IDUsuario;
                this.IDMiembro = IDMiembro;
                this.IDAsociado = IDAsociado;
                this.nombre = nombre;
                return result;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public void getMascotaRaza_Busca(int IDTipo)
        {
            wsPets.AuthHeaderValue = auth;
            MascotaRaza_Busca = wsPets.MascotaRaza_Busca(IDTipo);
        }
    }
}