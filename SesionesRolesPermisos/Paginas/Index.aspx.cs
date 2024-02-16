using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SesionesRolesPermisos.Paginas
{
    public partial class Index : System.Web.UI.Page
    {
        //Evento de carga de página que usamos para agregar el nombre de usuario al mensaje de Bienvenido
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NombreUsuario"] != null) // Verifica si el usuario está autenticado.
            {
                string nombreUsuario = Session["NombreUsuario"].ToString(); //Variable que contendrá el username obtenido de la variable de sesión.
                                                                            //La conversión a string es porque las variables de Session recuperan objetos de tipo Objects. 

                lblBienvenido.Text = "¡Bienvenido, " + nombreUsuario + "!"; //Le damos texto al label. Usamos la variable que almacena a la variable de sesión para que el texto mute según la sesión.
            }
        }
    }
    
}
