using SesionesRolesPermisos.Permisos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SesionesRolesPermisos.Paginas
{
    public partial class AdministrarPermisos : System.Web.UI.Page
    {

        //DESDE EL COMIENZO HASTA LA LÍNEA 236, TODOS LOS METODOS TENDRÁN FIN DE VISUALIZACIÓN. Manejará la lógica de los repeaters y de la propiedad Checked del Checkbox.

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //Al cargarse la página
            {
                rptRoles.DataSource = ObtenerDatosRoles(); //Al rptRoles se le da un origen de datos de la tabla Roles, para que pueda usar los valores de sus columnas.
                rptRoles.DataBind();

                //Solo damos origen de datos al rptRoles porque es el único al que se le debe hacer esto al instante que se carga la página. 
                //El rptFormularios que esta anidado en el de Roles se creá dinámicamente al ejecutarse el evento ItemDataBound (metodo a continuación)
            }
        }


        //Metodo que le da valor a la propiedad Checked del checkbox (chx) cada vez que se crea una nueva fila. 
        //Se buscarán los 3 argumentos requeridos para ejecutar el procedure que obtiene el valor de Permisos de la DB para capitalizarlo visualmente en el frontend.
        //Es en el metodo que se llamará que se ejecuta el procedure, no en este.
        protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) //Si el ItemTemplate del rptRoles es de tipo 'Item' o 'AlternatingItem'
            {
                DataRowView infoDelItem = e.Item.DataItem as DataRowView; // Almacenamos la info total del item (los valores de sus controles anidados) en un objeto DataRowView
                int idRol = Convert.ToInt32(infoDelItem["IdRol"]); //Obtenemos el valor del IdRol que se encuentra en el Label del rptRoles, el segundo controlador que posee. Usaremos IdRol como argumento del metodo que nos dará el valor que deberá tener Checked del chx desde la DB (columna Permisos).

                Repeater rptFormulario = e.Item.FindControl("rptFormularios") as Repeater; //Buscamos al rptFormularios en el ItemTemplate del rptRoles para encontrar el segundo argumento del metodo que necesitamos ejecutar.
                rptFormulario.DataSource = ObtenerDatosFormularios(); //Le damos al repeater la tabla Formularios como el origen de datos. 
                rptFormulario.DataBind();

                foreach(RepeaterItem itemTemplateRptFormularios in rptFormulario.Items) //Iteramos por cada ItemTemplate (fila) que se creará (serán 5 en total)
                {
                    int idFormulario = Convert.ToInt32((itemTemplateRptFormularios.FindControl("hdnIdFormulario") as HiddenField).Value); //Obtenemos el Value del HiddenField dentro del ItemTemplate del RptFormularios. Será el segundo argumento a usar en el metodo que nos dará el valor a asignarle a Checked.

                    CheckBox chxPermisos = itemTemplateRptFormularios.FindControl("chkPermisos") as CheckBox; //Asociamos el checkbox usando el metodo FindControl() en el ItemTemplate que lo posee. Esto se hace para luego de obtener el valor del Checked con el metodo, asignarselo. 

                    chxPermisos.Checked = ObtenerValorDeChecked(idRol, idFormulario); //Le asignamos el valor al checked. Lo obtenemos mediante un metodo, que es argumentando con los 2 argumentos obtenidos hasta ahora. Hace un select a la columna permisos y obtiene el valor en bit, que convertimos a bool. Se devuelve T || F.
                }
            }
        }



        //Metodo que obtiene el valor BIT de la columna Permisos, y luego lo trasnforma a BOOLEAN para poder enviarle ese valor al Checked del Checkbox. (Línea de codigo 48)
        private bool ObtenerValorDeChecked(int idRol, int idFormulario)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion()) //Creamos la conexion
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerCheckedPermisos", conexion)) //Creamos comando con procedure.
                {
                    cmd.CommandType = CommandType.StoredProcedure; //Lo tipamos como StoredProcedure.
                    cmd.Parameters.AddWithValue("@IdRol", idRol); //Argumentamos los parametros con las variables del 
                    cmd.Parameters.AddWithValue("@IdFormulario", idFormulario); // metodo anterior (que son los argumentos de este metodo)

                    conexion.Open(); //La abrimos

                    object result = cmd.ExecuteScalar(); //Obtenemos el resultado de la ejecución del procedure en una variable tipada con object. El resultado obtenido de la DB es tipo BIT, por lo tanto queda pediente la conversión a bool para que se aceptado por la propiedad Checked.

                    if (result != null && result != DBNull.Value) //Si obtuvimos un resultado..
                    {
                        return Convert.ToBoolean(result); //Lo convertimos a bool. Ya que el procedimiento lo devuelve en BIT, por el tipo de columna. Pero lo necesitamos en bool para usar en la propiedad Checked.
                    }
                }
            }
            return false;
        }

        //Query del procedimiento utilizado en el metodo
        //create procedure ObtenerCheckedPermisos
        //@IdRol int,  
        //@IdFormulario int  
        //as begin
        //select Permisos from Permisos
        //where IdRol = @IdRol and IdFormulario = @IdFormulario
        //end




        //CÓDIGO A IGNORAR POR HABER QUEDADO OBSOLETO. EXPLICA AL ITEMDATABOUND DEL RPTROLES
        //Las líneas de código clave son las q obtienen el resultado final y las q obtienen los argumentos para ejecutar el procedimiento q obtiene el valor de la DB (permisos).
        // int idRol = Convert.ToInt32(infoDelItem["IdRol"]); obtenemos el IdRol del label anidado en el ItemTemplate (la fila) del rptRoles, el principal.
        // int idFormulario = Convert.ToInt32((itemRptFormularios.FindControl("hdnIdFormulario") as HiddenField).Value); obtenemos el idFormulario buscando en el TemplateField (fila) del rptFormularios el HiddenField anidado que lo posee (al IdFormulario).
        // bool permisos = ObtenerValorDeChecked(idRol, idFormulario); Obtenemos el valor de la columna Permisos, que es convertido a bool.

        //Estos dos valores asi como el resto fueron obtenidos desde la DB con un Eval() en el código de front, Eval es argumentado con el nombre de ciertas columnas de una tabla específica, esto dentro de un elemento padre al que se le deberá dar un origen de datos de la tabla de la que obtienen la data los Eval, asi sabe con exactitud y no se confunde con columnas de diferentes tablas, esto se realizó en la línea de código 19.


        //Paso por paso: 
        //Verificamos que el tipo del item sea 'Item' o 'AlternatingItem' para asegurarse de que estás trabajando con elementos de datos reales y no encabezados o pies de página del Repeater, si lo es proseguimos. 

        //Creamos un objeto DataRowView que contendrá la información de cada fila. e.Item.DataItem === (evento.fila.infoDeLaFila) fila = <ItemTemplate>

        //Mediante notación de corchetes en este nuevo objeto obtenemos el "IdRol" que estaba dentro de un label invisible anidado, lo usaremos como primer argumento del metodo.

        //Creamos un nuevo objeto repeater y lo asociamos al repeater hijo del codigo base mediante e.Item.FindControl("rptFormularios"). Esto para obtener el segundo argumento del metodo que está allí dentro.

        //Le damos un origen de datos al repeater, obtendrá sus valores de la tabla formularios, que la obtenemos con un metodo, que ejecuta el procedure encargado del select.

        //Iteramos por las filas del repeater hijo con una variable temporal tipo RepeaterItem, obtenemos el idFormulario encontrando al control que lo almacenaba, el hiddenText

        //Creamos un objeto checkbox que representará al checkbox de la pagina base para eso lo encontramos usando la variable temporal con el mismo metodo y argumentado con su ID " fila.FindControl("chxPermisos") " Esto para luego poder cambiarle la propiedad de Checked, según el valor que devuela el metodo

        //Ejecutamos el metodo que devolverá un valor booleano, según tenga o no permisos, se almacena en una variable bool.

        //Inicializamos el el checkbox con la variable bool que almacenó el resultado del metodo. Si el resultado fue true, el checkbox estará checked, si no, no lo estará.

        //Este metodo se llamará 12 veces, ya que son 6 filas (6 formularios) por 2 roles.




        // e.Item.ItemType == ListItemType.Item
        // e.Item.ItemType accede a la propiedad tipo de item  
        // == ListItemType.Item inicializa la propiedad con el valor 'item'.

        // 'e' es un objeto de tipo RepeaterItemEventArgs, que es proporcionado automáticamente por el sistema cuando se dispara el evento ItemDataBound, sirve para usar las propiedades y metodos que 
        // 'e.Item' representa el elemento actual del Repeater que está siendo enlazado a los datos durante el evento.
        // 'e.ItemType' es una propiedad de RepeaterItemEventArgs que indica el tipo del elemento actual(Item, AlternatingItem, etc.).



        //



        //e.Item.DataItem accede a la data del item.
        //DataRowView infoFila = e.Item.DataItem as DataRowView;
        //e.Item.DataItem esto devuelve la información que posee la fila generada por el repeater, como se ve al final tambien se debe realizar una conversión a DataRowView, tal cual sucederá con el próximo caso que veremos, ya que todo lo que se obtiene de controladores, ya sea los datos (como en est caso) o los elementos enteros como veremos en el caso anterior se obtiene como tipo 'Object' la clase básica, para que no haya problemas de compatibilidad, pero ahora queremos almacenar los datos en un objeto de una clase específica (DataRowView) por lo tanto necesitamos la conversión a la clase correspondiente. 



        //



        //Repeater repeaterFormularios = e.Item.FindControl("rptFormularios") as Repeater;
        //e.Item.FindControl("rptFormularios") busca un control por su ID y lo crea como objeto con su clase. Se debe realizar la conversión con el alias "as" como se ve.

        // e.Item.FindControl("rptFormularios")
        // FindControl() es un metodo de la clase Control, pero no se necesita acceder a el través de e.Item.Control. Debes utilizar e.Item.FindControl directamente porque e.Item ya es un objeto de la clase Control. 

        //Repeater tiene dos tipos principales de elementos: Item y AlternatingItem.
        //Estos se utilizan para representar elementos individuales dentro de la lista generada por el Repeater.
        //La verificación if en el evento ItemDataBound está destinada a ejecutarse para ambos tipos de elementos: Item y AlternatingItem
        //ListItemType.Item: Representa un elemento normal de la lista.
        //ListItemType.AlternatingItem: Representa un elemento alternante de la lista.En el Repeater, los elementos alternantes tienen un estilo diferente para mejorar la legibilidad de la lista.



        //LINEA 56 & 57
        //DataRowView rowView = e.Item.DataItem as DataRowView;
        //e.Item es el contenedor visual(la fila) que se genera para cada elemento en la fuente de datos, el <ItemTemplate>, literalmente.
        //e.Item.DataItem son los datos reales que se asignan y visualizan en ese contenedor.
        //as DataRowView intenta convertir esos datos a un objeto de tipo DataRowView. El uso de as evita una excepción si el tipo de datos no es compatible con DataRowView.
        //rowView ahora es el nombre del objeto DataRowView que contiene los datos del elemento actual.


        //int IdRol = Convert.ToInt32(rowView["IdRol"]);
        //rowView["IdRol"] accede al valor de la columna "IdRol" en el objeto DataRowView que obtuvo esta columna de su label anidado.
        //Convert.ToInt32 convierte ese valor a un entero (int) y lo almacena en la variable IdRol.
        //Ahora, IdRol contiene el valor específico de la columna "IdRol" asociada a la fila actual del Repeater.





                    
        
                        //Metodos que obtienen los datos de las tablas Roles y Formularios para darle el DataSource a los Repeater´s

        //Metodo que obtiene los datos de la tabla Roles, su resultado será el DataSource del rptRoles, se usó en el Page_Load
        private object ObtenerDatosRoles()
        {
            DataTable datosDeRoles = new DataTable(); //Creamos un objeto DataTable (diseñado para almacenar datos de tablas)

            using (SqlConnection conexion = Conexion.ObtenerConexion()) //Creamos la conexión 
            {
                SqlCommand cmd = new SqlCommand("ObtenerDataRoles", conexion); //Creamos un comando que ejecutará un procedure de la DB.
                cmd.CommandType = System.Data.CommandType.StoredProcedure; //Tipamos el cmd como procedure

                SqlDataAdapter adapter = new SqlDataAdapter(cmd); // ejecutamos el cmd y almacenamos su resultado en un adaptador SqlDataAdapter
                adapter.Fill(datosDeRoles); //Rellenamos al objeto DataTable con los datos que almacenó el adaptador, objeto SqlDataAdapter.
            }

            return datosDeRoles; //Retornamos la info de la tabla Roles en el objeto DataTable que la almacena.
        }

        //Query del procedimiento
        // select * from Roles


        //Metodo que obtiene los datos de la tabla Formularios, su resultado será el DataSource del rptFormularios. Usado en el rptRoles_ItemDataBound
        private object ObtenerDatosFormularios()
        {
            DataTable datosDeFormularios = new DataTable(); // Objeto tipo DataTable que almacenará la info de la tabla Formularios

            using (SqlConnection conexion = Conexion.ObtenerConexion()) //Creamos la conexión
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerDataFormularios", conexion)) //Creamos el comando que ejecuta un procedimiento.
                {
                    cmd.CommandType = CommandType.StoredProcedure; //Tipamos al comando como procedimiento almacenado.

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd); //Objeto SqlDataAdapter que obtendrá la info de la tabla formularios al ejecutar el cmd.
                    adapter.Fill(datosDeFormularios); //Rellenamos el objeto DataTable con la  info del objeto SqlDataAdapter
                }
            }
            return datosDeFormularios; //Retornamos la info de la tabla Formularios en el objeto que la almacena.
        }

        //Query del procedimiento
        // select * from Formularios








        







                    
                        //LOS METODOS A CONTINUACIÓN TIENEN COMO FIN LA ACTUALIZACIÓN DE LOS PERMISOS LLEVADA A CABO POR EL ADMINISTRADOR. (UN UPDATE EN LA TABLA PERMISOS)

        //Metodo que identificará si quien quiso actualizar permisos es Administrador o Usuarios, accionará diferentes. Si es admin, actualizará los permisos, si es user, no.
        protected void btnGuardarPermisos_Click(object sender, EventArgs e)
        {
            if (Session["NombreUsuario"] != null) //Si la sesión sigue activa.
            {
                string nombreUsuario = Session["NombreUsuario"].ToString(); //Obtenemos el username de la variable de sesión, lo usaremos como argumento a continuación.
                int idRol = ConsultaDePermisos.ObtenerIdRol(nombreUsuario); //Obtenemos el IdRol del usuario de la sesión usando su nombre como argumento mediante el metodo correspondiente de la biblioteca de clases "ConsultaDePermisos".

                if (idRol== 1) //Si es una sesión de administrador..
                {
                    int filasAfectadas = ActualizarPermisos(); //Llevamos a cabo el update y almacenamos la cantidad de filas que se afectaron para saber si fue exitoso o no. El metodo que lleva a cabo la actualización devuelve las filas afectadas ya que usa un ExecuteNonQuery() 
                    MostrarMensajeAdministrador(filasAfectadas); //Mostramos el mensaje de éxito o error, según suceda, por eso se argumenta la cantidad de filas afectadas.
                }
                else //Si la sesion no es administrador
                {
                    MostrarMensajeUsuario(); // No actualizaremos los permisos y le mostraremos el mensaje de rechazo.
                }
            }
        }


        //Metodo que realiza un update en la tabla Permisos. Se ejecutará si es el Administrador (IdRol == 1) quien ejecuta el evento Click del boton que actualiza los permisos.
        private int ActualizarPermisos()
        {
            int actualizacionesTotales = 0; //Variable que almacenara cuantos permisos fueron modificados.

            foreach (RepeaterItem itemRptRoles in rptRoles.Items) //Iteramos por los items (ItemTemplate) del repeater principal (roles) para obtener el argumento necesitado.
            {
                int idRol = Convert.ToInt32((itemRptRoles.FindControl("lblIdRol") as Label).Text); //Obtenemos idRol para argumentarlo en el procedure. Lo obtenemos buscandolo con FindControl() mediante el ItemTemplate en donde se encuentra. Al final aclaramos que solo debeseamos tener el valor de su propiedad "Text".

                //Ahora necesitamos el otro argumento que es el IdFormulario, por lo tanto debemos buscar el control donde se encuentra (HiddenField):
                //Primero debemos encontrar acceder al ItemTemplate que contendrá ese HiddenField, para eso debemos buscarlo dentro del ItemTemplate del rptRoles:

                foreach (RepeaterItem itemRptFormulario in (itemRptRoles.FindControl("rptFormularios") as Repeater).Items)//Obtenemos el ItemTemplate del rptFormularios accediendo a el desde el rptFormularios que asi mismo accedimos a el mediante el ItemTemplate del rptRoles (el repeater principal), ya que el repeaterFormularios se encuentra dentro de este ItemTemplate.
                {
                    //Ya ubicados en el ItemTemplate del rptFormularios, podemos acceder a sus controles, en este caso al HiddenField que necesitamos.
                    int idFormulario = Convert.ToInt32((itemRptFormulario.FindControl("hdnIdFormulario") as HiddenField).Value); //Buscamos FindControl() el valor del HiddenField, es decir, el segundo argumento que necesitamos para realizar el UPDATE. Finalmente aclaramos que queremos obtener el valor de la propiedad "Value", que es la propiedad donde se encontrará el valor necesitado.

                    CheckBox checkBoxPermisos = itemRptFormulario.FindControl("chkPermisos") as CheckBox; //Obtenemos el checkbox para obtener su valor de la propiedad Checked y asi usarlo como tercer argumento.

                    bool permisos = checkBoxPermisos.Checked; //Obtenemos el valor que dejó el administrador en el checkbox antes de clickear en el boton "Guardar Permisos".
                                                              //Es el valor que se quedará en la columa Permisos de la tabla Permisos, el nuevo valor actualizado.

                    using (SqlConnection conexion = Conexion.ObtenerConexion()) //Obtenemos la conexión
                    {
                        using (SqlCommand cmd = new SqlCommand("ActualizarPermisos", conexion)) //Creamos el comando con el procedure.
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure; //Tipamos el comando como procedimiento almacenado.

                            cmd.Parameters.AddWithValue("@Permisos", permisos); //Le damos valor a los parametros del procedure.
                            cmd.Parameters.AddWithValue("@IdRol", idRol);
                            cmd.Parameters.AddWithValue("@IdFormulario", idFormulario);

                            conexion.Open(); //Abrimos la conexión para la ejecución del comando sql

                            int filasAfectadas = cmd.ExecuteNonQuery(); //Ejecutamos el comando y almacenamos la cantidad de filas que afectó en una variable.

                            conexion.Close(); //Cerramos la conexión

                            actualizacionesTotales += filasAfectadas; //Transferimos las filas afectadas a actualizacionesTotales, ya que esta es global y no local del using como lo es filasAfectadas.

                        }
                    }
                }
            }


            return actualizacionesTotales;
        }
        //Query del procedure usado en el metodo
        // create procedure ActualizarPermisos
        // @Permisos bit,  
        // @IdRol int,  
        // @IdFormulario int  
        // as begin  
        // if exists(select 1 from Permisos where IdRol = @IdRol and IdFormulario = @IdFormulario)
        // begin
        // update Permisos
        // set Permisos = @Permisos
        // where IdRol = @IdRol and IdFormulario = @IdFormulario
        // end  
        // end





        //4 METODOS QUE TENDRÁN QUE VER CON LOS MENSAJES DEL LABEL. LAS 3 POSIBILIDADES Y EL SCRIPT QUE OCULTARA AL LABEL.


        //Es el mensaje de éxito o error que se mostrará al administrador intentar modificar un permiso.
        private void MostrarMensajeAdministrador(int filasAfectadas)
        {
            if (filasAfectadas > 0) //Si se afectó una fila, se realizó la actualización correctamente.
            {
                lblMensaje.Text = "¡Permisos Actualizados!"; 
                lblMensaje.CssClass = "text-success";
            }
            else
            {
                lblMensaje.Text = "¡No se han podido actualizar los permisos!";
                lblMensaje.CssClass = "text-danger";
            }

            OcultarMensajeDespuesDeTiempo();
        }


        //Mensaje que se mostrará en caso de que el usuario (IdRol 2) intente modificar permisos.
        private void MostrarMensajeUsuario()
        {
            lblMensaje.Text = "Lo siento, solo el administrador Guillermo puede modificar permisos. Conformate con ver.";
            lblMensaje.CssClass = "text-danger";

            OcultarMensajeDespuesDeTiempo();
        }


        //Metodo que contiene sl script que hara que el lbl indicando una de las 3 posibles opciones (Admin si, admin no, usuario no) desaparezca despues de 3 segundos.
        private void OcultarMensajeDespuesDeTiempo()
        {
            string script = "<script type=\"text/javascript\">setTimeout(function(){ document.getElementById('" + lblMensaje.ClientID + "').style.display='none'; }, 3000);</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "HideMessage", script, false);
        }




    }

}
