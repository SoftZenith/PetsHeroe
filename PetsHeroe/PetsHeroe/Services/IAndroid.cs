using System;
using System.Data;
using System.Threading.Tasks;
using PetsHeroe.Model;

namespace PetsHeroe
{
    public interface IAndroid
    {
        DataTable CAM_Busca { get; set; }
        DataTable Ciudad_Busca { get; set; }
        bool Codigo_Valida { get; set; }
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
        Asociado ValidaUsuario { get; set; }
        DataTable MascotaRaza_Busca { get; set; }
        DataTable Veterinario_Registro { get; set; }
        bool EnviaContrasena { get; set; }
        bool Mascota_Registro { get; set; }
        bool Entrega_Localizacion { get; set; }

        /*Localización de mascota*/
        MensajeDueno Entrega_SoloMensaje { get; set; }


        int IDUsuario { get; set; }
        int IDMiembro { get; set; }
        int IDAsociado { get; set; }
        string nombre { get; set; }

        void getCAM_busca(double lat, double lon, double kms);
        void getCiudad_Busca(int IDEstado);
        void getCodigo_Valida(string codigo);
        void getEstado_Busca();
        void getEnviaContrasena(string correo);
        void getMarcaProducto_Busca();
        void getMascotaColor_Busca(int IDTipo);
        void getMascotaEstatus_Busca();
        void getMascotaTipo_Busca();
        void getMascotaRaza_Busca(int IDTipo);
        void getMascota_Registro(Dueno mascota);
        void getPais_Busca();
        void getProducto_Busca();
        void getServicio_Busca();
        void getTipoAsociado_Busca();
        void getTipoProducto_Busca();
        bool getValidaUsuario(String user, String pass);
        bool setVeterinario_Registro(Asociado asociado);
        void CloseApp();
        Retorno setEntrega_SoloMensaje(MensajeDueno mensaje);
        bool setEntrega_Localizacion(MensajeDueno localizacion);
        Task<bool> getPermisoLocation();
    }
}