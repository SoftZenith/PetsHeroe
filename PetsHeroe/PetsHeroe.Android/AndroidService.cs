using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
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
                return wsPets.Producto_Busca(-1, -1, idTipoProducto, idMarca, "", "", true);
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
                //return wsPets.Producto_Busca(-1, -1, -1, -1, "", UPC, false);
                return wsPets.PromoProductos_Busca(-1, -1, -1);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable getServicio_Busca(int idAsociado, int tipoMascota)
        {
            try
            {
                wsPets.AuthHeaderValue = auth;
                return wsPets.Servicio_Busca(-1, -1, tipoMascota, "", "");
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

        public Retorno getMascota_Registro(Dueno mascota)
        {
            var wsPetsRegistro = new wsPetsApp();

            wsPetsRegistro.AuthHeaderValue = auth;
            try
            {
                wsPetsRegistro.Mascota_Registro(mascota.mascostaCodigo, mascota.nombre, mascota.apellidoP, mascota.apellidoM, (char)mascota.sexo, mascota.correo,
                    mascota.contrasena, mascota.mascostaCodigo, mascota.nombreMascota, (char)mascota.sexoMascota, mascota.idTipoMascota, mascota.idRazaMascota,
                    mascota.idColorMascota, mascota.edadMascota, out int IDMiembro, out int IDCodigo, out int IDMascota, out int IDEstatusCodigo);
                Mascota_Registro = true;
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapExc)
            {
                string error = Retorno.xmlToStringMessage(soapExc.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }


        public Retorno setVeterinario_Registro(Asociado asociado)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.Veterinario_Registro(asociado.nombreComerial, asociado.nombre, asociado.apellidoPaterno, asociado.apellidoMaterno, (char)asociado.sexo,
                    asociado.correo, asociado.contrasena, asociado.tipoAsociado, out int IDAsociado);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapex)
            {
                string error = Retorno.xmlToStringMessage(soapex.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
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

        async public Task CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public void CloseAppSinc() {
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

        public int getIdMascota_Busca(string codigoMascota)
        {
            wsPets.AuthHeaderValue = auth;
            int idMascota = -1;
            try
            {
                Mascota_Busca = wsPets.Mascota_Busca(-1, -1, -1, -1, -1, -1, -1, "", codigoMascota, "");
                if (Mascota_Busca.Rows.Count != 1)
                {
                    return -1;
                }
                foreach (DataRow dr in Mascota_Busca.Rows) {
                    idMascota = Convert.ToInt32(dr["IDPet"].ToString());
                }
                return idMascota;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool getIdMascota_IdMember(string codigoMascota)
        {
            wsPets.AuthHeaderValue = auth;
            try {
                Mascota_Busca = wsPets.Mascota_Busca(-1, -1, -1, -1, -1, -1, -1, "", codigoMascota, "");
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public Retorno setMascota_Incidente(int idMascota,int tipoIncidente, int tipoRetorno, int condicion, string notas)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.Mascota_Incidente(idMascota, tipoIncidente, 5, condicion, tipoRetorno, -1, notas);
                return new Retorno() {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapexc) {
                string error = Retorno.xmlToStringMessage(soapexc.Detail.InnerXml);
                return new Retorno() {
                    Resultado = false,
                    Mensaje = error
                };

            }catch (Exception) {
                return new Retorno() {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
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

        public Retorno agregar_venta(int ticket, int idMascota, int idSucursal, int idProducto, int idServicio, int unidades, double costo, out int idTicketOut, out int ventaResult)
        {
            int IDTicket = ticket;
            wsPets.AuthHeaderValue = auth;
            try
            {
                ventaResult = wsPets.Venta(ref IDTicket, idMascota, idSucursal, idProducto, idServicio, unidades, Convert.ToDecimal(costo));
                idTicketOut = IDTicket;
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapExc)
            {

                string error = Retorno.xmlToStringMessage(soapExc.Detail.InnerXml);
                ventaResult = -1;
                idTicketOut = -1;
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                ventaResult = -1;
                idTicketOut = -1;
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }

        public Retorno ticketPaga(int idMascota, int idSucursal, int ticket, decimal puntosGastados)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.TicketPaga(idMascota, idSucursal, ticket, puntosGastados);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException exc)
            {
                string error = Retorno.xmlToStringMessage(exc.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error inesperado"
                };
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

        public Resultado promoProductos_Agrega(int idAsociado, int idProducto, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades, bool activa)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoProductos_Agrega(idAsociado, idProducto, nombre, precioRegular, precioPromo, desde.Date, hasta.Date, puntos, unidades, activa);
                return new Resultado()
                {
                    status = true,
                    errorMessage = ""
                };
            }
            catch (SoapException ex)
            {
                string error = ex.Detail.InnerText;
                return new Resultado()
                {
                    status = false,
                    errorMessage = error
                };
            }
        }

        public Resultado promoServicio_Agregar(int idAsociado, int idTipoServicio, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades, bool activa)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoServicios_Agrega(idAsociado, idTipoServicio, nombre, precioRegular, precioPromo, desde, hasta, puntos, unidades, activa);
                return new Resultado() {
                    status = true,
                    errorMessage = ""
                };
            }
            catch (SoapException ex)
            {
                string error = ex.Detail.InnerText;
                return new Resultado() {
                    status = false,
                    errorMessage = error
                };
            }
        }

        public bool promoProducto_Edita(int idPromocion, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades, bool activa) {

            wsPets.AuthHeaderValue = auth;
            try {
                wsPets.PromoProductos_Modifica(idPromocion, nombre, precioRegular, precioPromo, desde, hasta, puntos, unidades, activa);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public Retorno promoServicio_Edita(int idPromocion, string nombre, decimal precioRegular, decimal precioPromo, DateTime desde, DateTime hasta, int puntos, int unidades, bool activa)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoServicios_Modifica(idPromocion, nombre, precioRegular, precioPromo, desde, hasta, puntos, unidades, activa);
                return new Retorno
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (Exception ex)
            {
                return new Retorno
                {
                    Resultado = false,
                    Mensaje = ex.ToString()
                };
            }
        }

        public Retorno promoProducto_Eliminar(int idPromocion)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoProductos_Elimina(idPromocion);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapex)
            {
                string error = Retorno.xmlToStringMessage(soapex.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }

        public Retorno promoServicio_Eliminar(int idPromocion)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.PromoServicios_Elimina(idPromocion);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapex)
            {
                string error = Retorno.xmlToStringMessage(soapex.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }

        public DataTable ticketCarga(int IDTicket, int IDMascota, int IDSucursal)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                return wsPets.TicketCarga(IDSucursal, IDMascota, IDTicket);
            }
            catch (Exception) {
                return null;
            }
        }

        public int getSucursal(int idAsociado)
        {
            wsPets.AuthHeaderValue = auth;
            int idSucursal = -1;
            DataTable dataCAM = new DataTable();
            try
            {
                dataCAM = wsPets.CAM_Busca(-1, -1, -1, idAsociado, 0, 0, 500000);
                foreach (DataRow dr in dataCAM.Rows)
                {
                    idSucursal = Convert.ToInt32(dr["IDPartnerLocation"].ToString());
                }
                return idSucursal;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public Retorno setServicioAgrega(int idTipoMascota, string codigo, string nombreService) {
            wsPets.AuthHeaderValue = auth;
            try {
                wsPets.Servicio_Agrega(idTipoMascota, codigo, nombreService);
                return new Retorno {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (Exception ex) {
                return new Retorno
                {
                    Resultado = false,
                    Mensaje = ex.ToString()
                };
            }
        }

        public Retorno ventaCancela(int idVenta)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.VentaCancela(idVenta);
                return new Retorno() {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapExc) {
                string error = Retorno.xmlToStringMessage(soapExc.Detail.InnerXml);
                return new Retorno() {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex) {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }

        public Retorno ventaCambia(int idVenta, int unidades, decimal costo)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.VentaCambia(idVenta, unidades, costo);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException soapExc) {
                string error = Retorno.xmlToStringMessage(soapExc.Detail.InnerXml);
                return new Retorno() {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex) {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Ocurrió un error desconocido"
                };
            }
        }

        public Retorno reglonCancela(int IDVenta)
        {
            wsPets.AuthHeaderValue = auth;
            try
            {
                wsPets.RenglonCancela(IDVenta);
                return new Retorno()
                {
                    Resultado = true,
                    Mensaje = ""
                };
            }
            catch (SoapException SoapExc)
            {
                string error = Retorno.xmlToStringMessage(SoapExc.Detail.InnerXml);
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = error
                };
            }
            catch (Exception ex)
            {
                return new Retorno()
                {
                    Resultado = false,
                    Mensaje = "Error desconocido"
                };
            }
        }

    }
}