using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;


namespace SesionesRolesPermisos
{
    public partial class CrearFormularios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Realizará un insert en la tabla Formularios, solo accederá aquí el administador.
        protected void btnCrearFormulario_Click(object sender, EventArgs e)
        {
            string rutaFormulario = txtRutaFormulario.Text;  //El nombre y ruta de form que ingresará el admin en los txt se usará como argumentos
            string nombreFormulario = txtNombreFormulario.Text; // en el procedure que realizará el insert. Así que los obtenemos para su posterior uso.
            
            if (Session["NombreUsuario"] != null) //Verificamos que continúe la sesión del usuario.
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion()) //Abrimos la conexión
                {
                    using (SqlCommand cmd = new SqlCommand("InsertFormulario", conexion)) //Creamos el comando.
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //Le damos un tipo de procedimiento almacenado.
                        cmd.Parameters.AddWithValue("@RutaFormulario", rutaFormulario); // Le damos valores a los parametros para que argumente al procedure con ellos.
                        cmd.Parameters.AddWithValue("@NombreFormulario", nombreFormulario); //La ruta que debemos indicar es la que será la ruta absoluta (sin terminación).

                        conexion.Open(); //Abrimos la conexión 

                        int filasAfectadas = cmd.ExecuteNonQuery(); //Se ejecuta el comando con un ExecuteNonQuery() que devuelve las filas afectadas por el comando.
                                                                    //El íconico mensaje que se retorna al ejecutarse una query "x rows affected". Obtiene ese número.

                        conexion.Close(); //Cerramos la conexión

                        if (filasAfectadas > 0) //Si filas afectadas es mayor a 0, significa que se realizó la creación del form con éxito.
                        {
                            lblMensaje.Text = "Formulario registrado exitosamente, Guillermo!"; //Se agregará texto al lbl que no lo tenía hasta ahora indicando que se registro correctamente.

                            //script que esconderá el Text del lbl cada 1000 milisegundos (1 segundo).
                            string script = "<script type=\"text/javascript\">setTimeout(function(){ document.getElementById('" + lblMensaje.ClientID + "').style.display='none'; }, 1000);</script>";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "HideMessage", script, false);

                        }
                        else //Si filas afectadas no es mayor a 0 no se creo el formulario correctamente
                        {
                            lblMensaje.Text = "Form no registrado!"; //Se le agrega txt al label indicando la falla en la creación.
                            lblMensaje.CssClass = "text-danger"; //Se le cambia el color a rojo, acorde a un error.
                        }
                    }
                }
            }

        }
        
    }
}

//Query del procedimiento del metodo
//create procedure InsertFormulario    
//@RutaFormulario varchar(100),
//@NombreFormulario varchar(100)    
//as begin    
//insert into Formularios values(@RutaFormulario, @NombreFormulario);
//end