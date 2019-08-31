using System;
using System.Data;
using PetsHeroe.Droid;
using PetsHeroe.Droid.mx.com.petshero;
using PetsHeroe.Model;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidService))]
namespace PetsHeroe.Droid
{
    public class AndroidService : IAndroid
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
        public int IDUsuario { get; set; }
        public int IDMiembro { get; set; }
        public int IDAsociado { get; set; }
        public string nombre { get; set; }
        public bool EnviaContrasena { get; set; }
        public bool Mascota_Registro { get; set; }
        public DataTable Veterinario_Registro { get; set; }
        public MensajeDueno Entrega_SoloMensaje { get; set; }
        public bool Entrega_Localizacion { get; set; }

        wsPetsApp wsPets = new wsPetsApp();

        AuthHeader auth = new AuthHeader()
        {
            Usuario = "appcelmypets2019",
            Password = "RRW7G0ZiF4D1bUasqazmTg",
            IDUsuario = 0,
            IPAddress = "0"
        };

        public void getCAM_busca(double lat, double lon, double kms)
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
            catch (Exception ex) {
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
            TipoAsociado_Busca = wsPets.TipoAsociado_Busca();
        }

        public void getTipoProducto_Busca()
        {
            throw new NotImplementedException();
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
            wsPets.AuthHeaderValue = auth;
            try{
                Mascota_Registro = wsPets.Mascota_Registro(mascota.idDueno.ToString(), mascota.nombre, mascota.apellidoP, mascota.apellidoM, (char)mascota.sexo, mascota.correo,
                    mascota.contrasena, mascota.mascostaCodigo.ToString(), mascota.nombreMascota, (char)mascota.sexoMascota, mascota.idTipoMascota, mascota.idRazaMascota,
                    mascota.idColorMascota, mascota.edadMascota, out int IDMiembro, out int IDCodigo, out int IDMascotaCodigo, out int IDEstatusCodigo);
            }catch (Exception ex) {
                Console.WriteLine("Error al registrar: "+ex.ToString());
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
                return false;
            }
        }

        public bool setEntrega_SoloMensaje(MensajeDueno mensaje)
        {
            try
            {
                wsPets.Entrega_SoloMensaje(5, mensaje.codigo, mensaje.nombre, mensaje.correo, mensaje.telefono, mensaje.mensaje, mensaje.latitud, mensaje.longitud);
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex);
                return false;
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
    }
}