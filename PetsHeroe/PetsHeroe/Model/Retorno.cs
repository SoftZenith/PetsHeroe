using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace PetsHeroe.Model
{
    public class Retorno
    {

        public bool Resultado { get; set; }
        public string Mensaje { get; set; }

        public static string xmlToStringMessage(string xml) {
            /*
            xml = 
                   "<Error xmlns=\"PetsHero\">" +
                    "<ErrorNumber>1</ErrorNumber>" +
                    "<ErrorMessage>Código inválido</ErrorMessage>" +
                    "<ErrorSource>Mascota_Registro</ErrorSource>" +
                    "<LineNumber>1</LineNumber>" +
                   "</Error>";*/

            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Error";
            xRoot.Namespace = "PetsHero";
            xRoot.IsNullable = true;

            ErrorSOAP errorSoap = new ErrorSOAP();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ErrorSOAP),xRoot);
                errorSoap = (ErrorSOAP)xmlSerializer.Deserialize(stream);
            }
            catch (Exception ex) {
                errorSoap.ErrorMessage = "Ocurrió un error";
            }
            return errorSoap.ErrorMessage;
        }

    }
}
