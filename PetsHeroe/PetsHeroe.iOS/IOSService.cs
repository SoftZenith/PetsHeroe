using System;
using PetsHeroe.iOS;
using System.Data;
using PetsHeroe.iOS.mx.com.petshero;
using PetsHeroe.Model;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System.Threading.Tasks;
using PetsHeroe.Services;

[assembly: Xamarin.Forms.Dependency(typeof(IOSService))]
namespace PetsHeroe.iOS
{
    class IOSService : IWebService
    {
        public DataTable CAM_Busca { get; set; }
        public DataTable Ciudad_Busca { get; set; }
        public bool Codigo_Valida { get; set; }
        public DataTable Estado_Busca { get; set; }
        public DataTable MarcaProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable MascotaColor_Busca { get; set; }
        public DataTable MascotaEstatus_Busca { get; set; }
        public DataTable MascotaTipo_Busca { get; set; }
        public DataTable Pais_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Producto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable Servicio_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable TipoAsociado_Busca { get; set; }
        public DataTable TipoProducto_Busca { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Asociado ValidaUsuario { get; set; }
        public DataTable MascotaRaza_Busca { get; set; }

        //Variables out
        public int IDUsuario = -1;
        public int IDMiembro = -1;
        public int IDAsociado = -1;


        public string nombre { get; set; }
        public bool EnviaContrasena { get; set; }
        public DataTable Veterinario_Registro { get; set; }
        public bool Mascota_Registro { get; set; }
        public MensajeDueno Entrega_SoloMensaje { get; set; }
        public bool Entrega_Localizacion { get; set; }
        public DataTable Mascota_Busca { get; set; }
        public DataTable Cliente_Busca { get; set; }

        wsPetsApp wsPets = new wsPetsApp();
        AuthHeader auth = new AuthHeader()
        {
            Usuario = "appcelmypets2019",
            Password = "RRW7G0ZiF4D1bUasqazmTg",
            IDUsuario = 0,
            IPAddress = "0"
        };


        public void getCAM_busca(double lat, double lon, int kms)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                CAM_Busca = wsPets.CAM_Busca(-1, -1, -1, -1, lat, lon, kms);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }

        public void getCiudad_Busca(int IDEstado)
        {
            wsPets.AuthHeaderValue = auth;
            Ciudad_Busca = wsPets.Ciudad_Busca(IDEstado);
        }

        public void getCodigo_Valida(string codigo)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                Codigo_Valida = wsPets.Codigo_Valida(codigo, out int IDCodigo, out int IDEstatusCodigo, out int IDMascotaCodigo);
            }
            catch (Exception)
            {
                Codigo_Valida = false;
            }
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
            wsPets.AuthHeaderValue = auth;
            MascotaEstatus_Busca = wsPets.MascotaEstatus_Busca();
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
            TipoAsociado_Busca = wsPets.TipoAsociado_Busca();
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
                Asociado asociado = new Asociado();
                asociado.idUsuario = IDUsuario;
                asociado.idMiembro = IDMiembro;
                asociado.idAsociado = IDAsociado;
                asociado.nombre = nombre;
                ValidaUsuario = asociado;
                return result;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public void getMascotaRaza_Busca(int IDTipo)
        {
            wsPets.AuthHeaderValue = auth;
            MascotaRaza_Busca = wsPets.MascotaRaza_Busca(IDTipo);
        }

        public void getEnviaContrasena(string correo)
        {
            wsPets.AuthHeaderValue = auth;
            try{
                wsPets.EnviaContrasena(correo);
                EnviaContrasena = true;
            }catch (Exception){
                EnviaContrasena = false;
            }
        }

        public bool setVeterinario_Registro(Asociado asociado)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                return wsPets.Veterinario_Registro(asociado.nombreComerial, asociado.nombre, asociado.apellidoPaterno, asociado.apellidoMaterno, (char)asociado.sexo,
                    asociado.correo, asociado.contrasena, asociado.tipoAsociado, out int IDAsociado);
            }
            catch (Exception) {
                return false;
            }
        }

        public void getMascota_Registro(Dueno mascota)
        {
            var wsPetsRegistro = new wsPetsApp();

            wsPetsRegistro.AuthHeaderValue = auth;
            try{
                wsPetsRegistro.Mascota_Registro(mascota.mascostaCodigo, mascota.nombre, mascota.apellidoP, mascota.apellidoM, (char)mascota.sexo, mascota.correo,
                    mascota.contrasena, mascota.mascostaCodigo, mascota.nombreMascota, (char)mascota.sexoMascota, mascota.idTipoMascota, mascota.idRazaMascota,
                    mascota.idColorMascota, mascota.edadMascota, out int IDMiembro, out int IDCodigo, out int IDMascota, out int IDEstatusCodigo);
                Mascota_Registro = true;
            }catch (Exception ex){
                Console.WriteLine("Error al registrar mascota: "+ex);
                Mascota_Registro = false;
            }
        }

        public Retorno setEntrega_SoloMensaje(MensajeDueno mensaje)
        {
            try
            {
                wsPets.Entrega_SoloMensaje(5, mensaje.codigo, mensaje.nombre, mensaje.correo, mensaje.telefono, mensaje.mensaje, mensaje.latitud, mensaje.longitud);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = "Se envio mensaje al dueño"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = ex.ToString()
                };
            }
        }

        public bool setEntrega_Localizacion(MensajeDueno localizacion)
        {
            try
            {
                wsPets.Entrega_Localizacion(5, localizacion.codigo, localizacion.idCiudad, localizacion.nombre, localizacion.correo, localizacion.telefono, localizacion.localizacion, localizacion.notas, localizacion.latitud, localizacion.longitud);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return false;
            }
        }

        public bool setEntrega_CAM(string codigo, string notas, double longitud, double latitud)
        {
            try
            {
                wsPets.Entrega_CAM(6, codigo, -1, notas, latitud, longitud);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> getPermisoLocation()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (permissionStatus != PermissionStatus.Granted)
            {
                return false;
            }
            return true;
        }

        public bool getMascota_Busca(int idMiembro)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                Mascota_Busca = wsPets.Mascota_Busca(-1, -1, -1, -1, -1, -1,idMiembro, "", "", "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CloseApp()
        {
            
        }

        public bool setMascota_Incidente(int idMascota, int tipoIncidente, int tipoRetorno, int condicion, string notas)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.Mascota_Incidente(idMascota, tipoIncidente, 6, condicion, tipoRetorno, -1, notas);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable setPuntosPromociones_Busca(int idMiembro, int idAsociado, int idMascota)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                return wsPets.PuntosPromociones_Busca(idMiembro, idAsociado, idMascota);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool getCliente_Busca(int idMiembro, int idAsociado)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                Cliente_Busca = wsPets.Cliente_Busca(idMiembro, idAsociado, -1, "", "", "", "", "", "", "", "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool getClientes_Busca(string codigo, string correo, string nombre)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                Cliente_Busca = wsPets.Cliente_Busca(-1, -1, -1, codigo, "", nombre, "", "", "", correo, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable getPromoProductos_Busca(int idAsociado)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                return wsPets.PromoProductos_Busca(idAsociado, -1);
            }
            catch (Exception ex) {
                return null;
            }
        }

        public DataTable getPromoServicios_Busca(int idAsociado)
        {
            wsPets.AuthHeaderValue = auth;
            try {
                return wsPets.PromoServicios_Busca(idAsociado, -1);
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}