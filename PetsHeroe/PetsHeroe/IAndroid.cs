using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PetsHeroe
{
    public interface IAndroid
    {
        DataTable CAM_Busca { get; set; }
        DataTable Ciudad_Busca { get; set; }
        DataTable Codigo_Valida { get; set; }
        DataTable Estado_Busca { get; set; }
        DataTable MarcaProducto_Busca { get; set; }
        DataTable MascotaColor_Busca { get; set; }
        DataTable MascotaEstatus_Busca { get; set; }
        DataTable MascotaTipo_Busca { get; set; }
        DataTable Pais_Busca { get; set; }
        DataTable Producto_Busca { get; set; }
        DataTable Servicio_Busca { get; set; }
        DataTable TipoAsociado_Busca { get; set; }
        DataTable TipoProducto_Busca { get; set; }
        DataTable ValidaUsuario { get; set; }
        DataTable MascotaRaza_Busca { get; set; }

        int IDUsuario { get; set; }
        int IDMiembro { get; set; }
        int IDAsociado { get; set; }
        string nombre { get; set; }

        void getCAM_busca();
        void getCiudad_Buscs();
        void getCodigo_Valida();
        void getEstado_Busca();
        void getMarcaProducto_Busca();
        void getMascotaColor_Busca(int IDTipo);
        void getMascotaEstatus_Busca();
        void getMascotaTipo_Busca();
        void getMascotaRaza_Busca(int IDTipo);
        void getPais_Busca();
        void getProducto_Busca();
        void getServicio_Busca();
        void getTipoAsociado_Busca();
        void getTipoProducto_Busca();
        bool getValidaUsuario(String user, String pass);
    }
}
