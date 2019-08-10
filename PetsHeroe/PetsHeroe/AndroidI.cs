using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PetsHeroe
{
    public interface AndroidI
    {

        DataTable tipoAsociado { get; set; }
        void getTipoSocios();


    }
}
