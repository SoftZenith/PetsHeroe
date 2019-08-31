using System;
namespace PetsHeroe.Model
{
    public class Asociado
    {
        public int idAsociado { get; set; }
        public int idUsuario { get; set; }
        public int idMiembro { get; set; }
        public int tipoAsociado { get; set; }
        public string nombreComerial { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int sexo { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
    }
}
