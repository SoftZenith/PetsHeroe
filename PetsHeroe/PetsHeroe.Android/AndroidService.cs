using System;
using System.Data;
using System.Threading.Tasks;
using PetsHeroe.Droid;
using PetsHeroe.Droid.mx.com.petshero;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidService))]
namespace PetsHeroe.Droid
{
    public class AndroidService : IWebService
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
        public bool Mascota_Registro { get; set; }
        public DataTable Veterinario_Registro { get; set; }
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
            try{
                CAM_Busca = wsPets.CAM_Busca(-1, -1, -1, -1, lat, lon, kms);
            }catch (Exception ex) {
                Console.WriteLine("Error: "+ex.ToString());
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
            catch (Exception) {
                Codigo_Valida = false;
            }
        }

        public void getEstado_Busca()
        {
            wsPets.AuthHeaderValue = auth;
            Estado_Busca = wsPets.Estado_Busca(1);
        }

        public DataTable getMarcaProducto_Busca()
        {
            wsPets.AuthHeaderValue = auth;
            return wsPets.MarcaProducto_Busca();
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

        public DataTable getProducto_Busca(int idAsociado, int idTipoProducto, int idMarca)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                return wsPets.Producto_Busca(idAsociado, -1, idTipoProducto, idMarca, "", "", false);
            }
            catch (Exception ex) {
                return null;
            }
        }

        public DataTable getProducto_Busca(int idAsociado, string UPC)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                return wsPets.Producto_Busca(idAsociado, -1, -1, -1, "", UPC, false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getServicio_Busca(int tipoMascota)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                return wsPets.Servicio_Busca(-1, -1, tipoMascota, "", false);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void getTipoAsociado_Busca()
        {
            wsPets.AuthHeaderValue = auth;
            TipoAsociado_Busca = wsPets.TipoAsociado_Busca();
        }

        public DataTable getTipoProducto_Busca()
        {
            try {
                wsPets.AuthHeaderValue = auth;
                return wsPets.TipoProducto_Busca();
            }
            catch (Exception ex) {
                return null;
            }
        }

        public bool getValidaUsuario(string user, string pass)
        {
            try{
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
            }catch (Exception ex){
                Console.WriteLine(ex.ToString());
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

        public void getMascota_Registro(Dueno mascota)
        {
            var wsPetsRegistro = new wsPetsApp();

            wsPetsRegistro.AuthHeaderValue = auth;
            try
            {
                wsPetsRegistro.Mascota_Registro(mascota.mascostaCodigo, mascota.nombre, mascota.apellidoP, mascota.apellidoM, (char)mascota.sexo, mascota.correo,
                    mascota.contrasena, mascota.mascostaCodigo, mascota.nombreMascota, (char)mascota.sexoMascota, mascota.idTipoMascota, mascota.idRazaMascota,
                    mascota.idColorMascota, mascota.edadMascota, out int IDMiembro, out int IDCodigo, out int IDMascota, out int IDEstatusCodigo);
                Mascota_Registro = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar mascota: " + ex);
                Mascota_Registro = false;
            }
        }

        public bool setVeterinario_Registro(Asociado asociado)
        {
            wsPets.AuthHeaderValue = auth;
            try{
                return wsPets.Veterinario_Registro(asociado.nombreComerial, asociado.nombre, asociado.apellidoPaterno, asociado.apellidoMaterno, (char)asociado.sexo,
                    asociado.correo, asociado.contrasena, asociado.tipoAsociado, out int IDAsociado);
            }catch (Exception ex){
                Console.WriteLine("Error en registro: "+ex);
                return false;
            }
        }

        public Retorno setEntrega_SoloMensaje(MensajeDueno mensaje)
        {
            try
            {
                wsPets.Entrega_SoloMensaje(5, mensaje.codigo, mensaje.nombre, mensaje.correo, mensaje.telefono, mensaje.mensaje, mensaje.latitud, mensaje.longitud);
                return new Retorno() {
                    Resultado = true,
                    Mensaje = "Se envio mensaje al dueño"
                };
            }
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex);
                return new Retorno() {
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
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex);
                return false;
            }
        }

        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public async Task<bool> getPermisoLocation()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (permissionStatus != PermissionStatus.Granted) {
                return false;
            }
            return true;
        }

        public bool setEntrega_CAM(string codigo, string notas, double longitud, double latitud)
        {
            try
            {
                wsPets.Entrega_CAM(5, codigo, -1, notas, latitud, longitud);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool getMascota_Busca(int idMiembro)
        {
            wsPets.AuthHeaderValue = auth;
            try {
                Mascota_Busca = wsPets.Mascota_Busca(-1, -1, -1, -1, -1, -1, idMiembro, "", "", "");
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool getIdMascota_Busca(string codigoMascota)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                Mascota_Busca = wsPets.Mascota_Busca(-1, -1, -1, -1, -1, -1, -1, "", codigoMascota, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool setMascota_Incidente(int idMascota,int tipoIncidente, int tipoRetorno, int condicion, string notas)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.Mascota_Incidente(idMascota, tipoIncidente, 5, condicion, tipoRetorno, -1, notas);
                return true;
            }catch (Exception) {
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
            catch (Exception) {
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
                //return wsPets.PromoProductos_Busca(idAsociado, -1);
                return wsPets.PromoProductos_Busca(idAsociado, -1, -1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getPromoServicios_Busca(int idAsociado)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                //return wsPets.PromoServicios_Busca(idAsociado, -1);
                return wsPets.PromoServicios_Busca(idAsociado, -1, -1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void agregar_venta(int ticket, int idMascota, int idSucursal, int idProducto, int idServicio, int unidades, double costo, out int idTicketOut, out int ventaResult)
        {
            int IDTicket = ticket;
            wsPets.AuthHeaderValue = auth;
            try {
                ventaResult = wsPets.Venta(ref IDTicket, idMascota, idSucursal, idProducto, idServicio, unidades, Convert.ToDecimal(costo));
                idTicketOut = IDTicket;
            }
            catch (Exception ex) {
                ventaResult = -1;
                idTicketOut = -1;
            }
        }

        public bool ticketPaga(int idMascota, int idSucursal, int ticket, decimal puntosGastados)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.TicketPaga(idMascota, idSucursal, ticket, puntosGastados);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public int producto_Agrega(int idTipoProducto, int idMarca, string nombre, string UPC)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                return wsPets.Producto_Agrega(idTipoProducto, idMarca, nombre, UPC);
            }
            catch (Exception ex) {
                return -1;
            }
        }

        public bool promoProductos_Agrega(int idAsociado, int idProducto, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoProductos_Agrega(idAsociado, idProducto, nombre, precioRegular, precioPromo, desde, hasta, puntos, unidades);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public bool promoServicio_Agregar(int idAsociado, int idTipoServicio, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoServicios_Agrega(idAsociado, idTipoServicio, nombre, precioRegular, precioPromo, desde, hasta, puntos, unidades);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}