using SesionesRolesPermisos.Permisos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace SesionesRolesPermisos.Paginas
{
    public partial class PaginaMaestra : System.Web.UI.MasterPage
    {
        //Evento que comprueba si el rol tiene permisos de acceder a la página. (Index, CrearFormularios, Administrar Permisos).

        //Tiene 6 posibilidades totales, 2 roles por 3 formularios. 
        //Deberá devolver 4 veces TRUE y 2 veces FALSE. Ya que el rol Admin Permisos = 1 en 3/3, mientras que el rol Usuario Permisos = 1 en 1/3.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["NombreUsuario"] != null) // Si hay una sesión activa.. Si la variable de sesión tiene valor, un usuario lleno el txt de entrada y se logueo. 
            {
                //Obtenemos los argumentos que necesitaremos para ejecutar el metodo que comprobará si tiene permisos
                string nombreUsuario = Session["NombreUsuario"].ToString(); //Obtenemos el valor de la variable de sesión (que obtuvo el valor del input de entrada del form IniciarSesion.aspx), las var de sesion manejan type Object, por eso conversión a string.
                string nombreFormulario = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath); //Obtenemos la ruta absoluta, y argumentando el resultado en un metodo eliminamos su extensión ".aspx". Esta eliminación se debe a que TienePermisos() ejecuta una query que tiene como usa como condicion a la columna "NombreFormulario" que no poseee la terminación, no "RutaFormulario" que si la posee. Si se hubieseusado la columna "RutaFormulario" hubiera bastado con "Request.Url.AbosultePath".
                //Las URL son objetos de la clase URI. Por eso podemos usar metodos como AbsolutePath, o precederlos de un Request.


                //Usaremos los valores obtenidos recientemente como argumentos del metodo a continuación que retornará un valor booleano.
                if (!ConsultaDePermisos.TienePermisos(nombreUsuario, nombreFormulario)) //Si retorna false..
                {
                    Response.Redirect("~/Paginas/AccesoDenegado.aspx"); //Al usuario se le niega el acceso.
                }
            }

        }
        //ViewState["TemaOscuro"] = true; //Declaramos una variable ViewState para llamar "Tema Oscuro" al estado por defecto de los forms. 
            //Habrá un handler de un boton para cambiar el tema que rotara entre los dos ViewState, el declarado recién y el que se creará en su handler. (Línea codigo 118)

        //TienePermisos() encuentra el IdRol y IdFormulario del usuario y formulario argumentados del nombre de usuario (txt log in) y formulario recibido (Request.Url.AbPath).
        //Estos valores los argumenta en un procedure que se ejecutará en un cmd, la query del procedure busca en la tabla Permisos si la columna Permisos de la fila que coincida con los id´s obtenidos es 1, si lo es CUENTA, y el valor devuelto es 1, si no lo es, no cuenta. La query es la siguiente:
        //  select COUNT(*) from Permisos  
        //  where IdRol = @IdRol and IdFormulario = @IdFormulario and Permisos = 1;
        //Se almacena el resultado de la busqueda en una variable, si es mas que 0 returna true, si no false.

        // Request.Url.AbsolutePath
        // Request es una propiedad de la clase HttpContext. Cuando accedes a esta propiedad obtienes una instancia de la clase 'HttpRequest'.
        // HttpRequest es una clase que representa la información específica de la solicitud HTTP, y tiene propiedades como Url, Headers, etc.
        // Url es una propiedad de la clase HttpRequest. Al acceder a ella obtienes un objeto de tipo URI, que es la URL de la solicitud HTTP.
        // AbsolutePath es una propiedad de la clase URI que devuelve la ruta absoluta del objeto URI (la URL).
        // La ruta absoluta es parte de la URL que sigue al nombre de dominio y al puerto. Por ej:
        // https://www.ejemplo.com:8080/pagina/ejemplo.aspx
        // https es el esquema.
        // "www.ejemplo.com" es el nombre de dominio.
        // "8080" es el número de puerto.
        // "/pagina/ejemplo.aspx" es la ruta absoluta.
        //En esta caso es una solicitud GET. La acción que la ejecuta es la solicitud realizada por el cliente al servidor para acceder a una nueva URL (formulario).

        // System.IO.Path.GetFileNameWithoutExtension()
        // Función que obtiene nombres de archivos sin la terminación, en nuestro caso la usamos para borrar el .aspx de la ruta absoluta obtenida con su argumentación.

        //En resúmen en la línea de código 20 realizamos un request a la URL (objeto URI) mediante un metodo de esa clase (URI). Y luego con la función GetFile.. eliminamos su terminación para obtener su nombre limpio ya que lo usaremos como argumento de un parametro que se usará en la condición de la query de un procedure de los metodos secundarios que encontrarán los ID´S, es decir que tiene que tener el nombre de una columna específica que será parte de la condición de la query.
        //La query es: 
        //  select IdFormulario from Formularios  
        //  where NombreFormulario = @NombreFormulario



        //Evento click de un boton que cerrará la sesión.
        protected void CerrarSesion_Click(object sender, EventArgs e)
        {
            if (Session["NombreUsuario"] != null) //Si el usuario sigue en sesión
            {
                FormsAuthentication.SignOut(); //Cerramos la sesión.
                Session.Clear(); //La limpiamos
                Session.Abandon(); //La abandonamos

                Response.Redirect("~/Paginas/IniciarSesion.aspx"); //Redireccionamos al usuario deslogueado a la página de log in.
            }
        }

        //FormsAuthentication es una clase de ASP.NET que contiene metodos que trabajan con la autenticación de formularios.
        //Ejs: 
        //    SignOut(); Cierra la sesión.
        //    SetAuthCookie(nombreUsuario, false); Crea una cookie de autenticación (Línea 36 del code behind de IniciarSesion.aspx)



        //Metodo del boton que cambiara el tema de oscuro a claro.
        protected void CambiarTema_Click(object sender, EventArgs e)
        {

                                                    //UBICAMOS LOS INPUTS A MODIFICAR

            //Mediante el ID obtendremos los 3 elementos de .Master a los que se le cambiará el color al cambiar el valor el ViewState (body, navbar, footer)
            HtmlGenericControl body = (HtmlGenericControl)this.FindControl("idBody"); //"this" ya que el body no tiene un elemento padre por el que acceder a el.

            HtmlGenericControl navbar = (HtmlGenericControl)body.FindControl("idNavbar"); //mediante el body obtenido encontramos la navbar anidada dentro de el.

            HtmlGenericControl footer = (HtmlGenericControl)body.FindControl("idFooter"); //encontramos al footer anidado en el body.

            ImageButton imageButton = (ImageButton)sender; //Asociamos la imgButton para modificar su imagen. Ya que es el argumento de este metodo, no debemos encontrarlo y usamos su nombre identificatorio "sender".
            
            //ContentPlaceHolder content = (ContentPlaceHolder)this.FindControl("Content3"); // FRACASO

            //Label lblBienvenido = (Label)content.FindControl("lblBienvenido"); // FRACASO

                                                        //MODIFICACIONES SEGUN EL VIEWSTATE

            //Declaramos un ViewState que representará el tema actual. Su valor inicial será true. Es decir que true representará al color predeterminado (oscuro).
            bool temaInicial = (ViewState["TemaPaginaMaestra"] as bool?) ?? true;

            if (temaInicial) //Entonces si es True cambiamos a oscuro y la img del imgBtn.
            {
                body.Style["background-color"] = "black"; //modificamos los atributos de ciertos elementos
                navbar.Attributes["class"] = "navbar navbar-expand-lg navbar-light bg-light";
                footer.Attributes["class"] = "mt-auto py-3 text-center bg-light text-dark";
                //lblBienvenido.Style["color"] = "white";
                imageButton.ImageUrl = "~/Gokuu.png";
            }
            else //Si es false cambiamos a claro y la img del imgBtn.
            {
                body.Style["background-color"] = "white";
                navbar.Attributes["class"] = "navbar navbar-expand-lg navbar-dark bg-dark";
                footer.Attributes["class"] = "mt-auto py-3 text-center bg-dark text-light";
                //lblBienvenido.Style["color"] = "black";
                imageButton.ImageUrl = "~/Narutoo.png"; 
            }

            ViewState["TemaPaginaMaestra"] = !temaInicial; //Invertimos el valor del ViewState mediante ! y su valor actual almacenado en temaOscuro. Esto para que en el próximo evento click se ejecute la otra bifuracación de código, es decir el otro tema ya que la variable temaOscuro tendra el otro valor, false. Asi funcionará ilimitadamente el cambio de tema. 
        }   
    }
}