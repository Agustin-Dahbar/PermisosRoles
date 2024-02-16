using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SesionesRolesPermisos.Paginas
{
    public partial class IniciarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Evento click del boton que inicia sesion. Comprobaremos si las credenciales ingresadas por el usuario son correctas buscando con un procedure esos valores en la misma fila en las diferentes columnas.
        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text; //Almacenamos en variables el nombre de usuario y la contraseña
            string contraseña = txtContraseña.Text; // que ingresó el usuario para comprobar si existen en la DB.

            using(SqlConnection conexion = Conexion.ObtenerConexion()) //Creamos una conexión
            {
                using(SqlCommand cmd = new SqlCommand("VerificarInicioDeSesion", conexion)) //Creamos comando que ejecuta un procedimiento.
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; // Tipamos al comando como StoredProcedure
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario); // Le damos argumentos al procedimiento, inicializando sus parametros. 
                    cmd.Parameters.AddWithValue("@Contraseña", contraseña); //estos argumentos serán los valores obtenidos de los txt de entrada.

                    conexion.Open(); //Abrimos la conexión (necesario para la ejecución del cmd, no para su declaración)

                    SqlDataReader reader = cmd.ExecuteReader(); //Almacenamos lo obtenido en el reader (no importa el contenido, solo nos importa saber si se obtuvó algo)

                    if (reader.Read()) //Si el reader obtuvo información, se encontró la fila con esos datos en la tabla Usuarios, es decir que las credenciales son correctas.
                    {

                        FormsAuthentication.SetAuthCookie(nombreUsuario, false);
                        //Creamos una cookie de autenticación, registrará la sesión del usuario y seguirá su actividad en el proyecto.
                        //FormsAuthenticaction es una clase y SetAuthCookie un metodo de ella, que crea una cookie.
                        //Argumentamos la creación de la cookie con el nombre de usuario para que lo asocie a la sesión logueada.
                        //El valor booleano indica si la cookie de autenticación seguirá activa al acabar la sesión actual del navegador o se desactivará.
                        //Si sigue activa (TRUE) el usuario no deberá volver a iniciar sesión aunque se cierre el navegador. Si es false, se deberá reloguear.
                        //Modifique el valor bool del segundo argumento y al cerrar el proyecto tuve que reloguearme, será que esto solo aplica siempre y cuando el proyecto se mantenga abierto y solo se cierre la pestaña, cosa imposible en mi caso con estas ejecuciones locales. Creo que debería subir este proyecto a la web para comprobarlo. 


                        Session["NombreUsuario"] = nombreUsuario;
                        //Creamos una variable de sesión que almacenará el nombre del usuario.
                        //Las variables de sesión sirven para retener información que el usuario haya proporcionado durante el resto de la sesión. 
                        //Esto podría ser útil para mejorar la experiencia del usuario en X pestaña de nuestra página web.
                        //La usaremos en el label del index para que su Bienvenido sea dinámico según quien inicie sesión.

                        Response.Redirect("~/Paginas/Index.aspx"); //Redireccionamos al usuario a la pagina principal.
                    }
                    else //Si no se obtuvo nada en el reader.
                    {
                        reader.Close(); //Lo cerramos.
                        lblMensajeError.Visible = true; 
                        lblMensajeError.Text = "Credenciales incorrectas";
                        btnStopLabel.Visible = true;
                        //Mostramos el mensaje de error y un boton para esconderlo.
                    }
                }
            }
        }

        //Query del procedimiento usado en el metodo
        //
        //create procedure VerificarInicioDeSesion
        //@NombreUsuario varchar(50),  
        //@Contraseña varchar(50)
        //as begin
        //select * from Usuarios
        //where NombreUsuario = @NombreUsuario and Contraseña = @Contraseña
        //end


        //Evento click del boton que pará al lbl que lanzá el mensaje de error 
        protected void btnStopLabel_Click(object sender, EventArgs e)
        {
            lblMensajeError.Visible = false; //Escondemos al label con el mensaje
            btnStopLabel.Visible = false; // y al boton.
        }
    }
}