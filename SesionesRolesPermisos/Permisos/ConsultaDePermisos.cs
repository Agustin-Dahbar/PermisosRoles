using System;
using System.Data.SqlClient;

namespace SesionesRolesPermisos.Permisos
{
    public class ConsultaDePermisos
    {
        //Metodo que cuenta (COUNT()) si un Rol tiene permisos en X formulario (es decir si la col Permisos = 1 en la tabla Permisos de la DB, ejecuta un COUNT() ).
        //Se ejecutará cada vez que se intente acceder a un form que heredó de la página maestra, ya que este metodo es llamado en el Page_Load del .Master. 
        internal static bool TienePermisos(string nombreUsuario, string nombreFormulario) //tipada con bool porque retornará true o false.
        {
            int idRol = ObtenerIdRol(nombreUsuario); //La query de este metodo usa los ID´s como condición, no los nombres. Por lo tanto los usamos como argumento en otros
            int idFormulario = ObtenerIdFormulario(nombreFormulario); //metodos con otras querys que se encargan de obtener los ID´s necesitados para el procedure "CuentaSiTienePermisos"

            if (idRol > 0 && idFormulario > 0) //Si los metodos encontraron valores reales significa que salió todo correcto, entonces proseguimos.
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion()) //Conexión para argumentarla en el comando.
                {
                    using(SqlCommand cmd = new SqlCommand("CuentaSiTienePermisos", conexion)) //Comando que ejecuta un procedure de la DB conectándose a ella.
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //Tipamos el comando como procedimiento para informarle que es el primer argumento.
                        cmd.Parameters.AddWithValue("@IdRol", idRol); //Usamos los argumentos obtenidos al comienzo del código con los metodos secundarios 
                        cmd.Parameters.AddWithValue("@IdFormulario", idFormulario); //para darle valor a los parametros del procedure

                        conexion.Open(); //Abrimos la conexión para ejecutar el cmd. 

                        int permisos = Convert.ToInt32(cmd.ExecuteScalar()); //Ejecutamos el cmd con Scalar, que su particularidad es la de devolver un solo valor.

                        return permisos > 0; //Retorno con condición booleana. La condición se evalúa y devuelve TRUE si se cumple. Si no, continúa el flujo natural y llega al else que devuelve false.
                                             
                        //Si permisos es mayor a 0 el procedure contó, por lo tanto se cumplió la condición clave "Permisos = 1" del procedure, es decir que el usuario tiene permisos. Retorna TRUE. El siguiente paso de este resultado es ser evaluado por el if en la línea de código 30 del code behind del .Master

                    }
                }
            }
            else //Si permisos es es == o menor a 0, el procedimiento no conto, entonces no tiene permisos (Permisos = 0)
            {
                return false; 
            }

        }

        //Query del procedimiento del metodo
        //create procedure CuentaSiTienePermisos  
        //@IdRol int,  
        //@IdFormulario int  
        //as begin   
        //select COUNT(*) from Permisos  
        //where IdRol = @IdRol and IdFormulario = @IdFormulario and Permisos = 1;  
        //end



        //Metodo secundario que usaremos en el metodo principal para obtener uno de los dos argumentos que necesitará el procedure del metodo principal.
        internal static int ObtenerIdRol(string nombreUsuario) //Tipada con INT porque retornará un INT, el ID.
        {
            using(SqlConnection conexion = Conexion.ObtenerConexion()) //Creamos la conexión.
            {
                using(SqlCommand cmd = new SqlCommand("ObtenerIdRol", conexion)) //Creamos el cmd que ejecutará al proceedure para la obtención del argumento requerido.
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; //Indicamos que el cmd tendrá un StoredProcedure modificando su tipado.
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);// Indicamos que el valor del parametro sea el nombreDeUsuario que se obtuvo en la línea de código 25 del code behind del .Master 

                    conexion.Open(); //necesario para ejecutar el cmd.

                    object result = cmd.ExecuteScalar(); //Ejecutamos el cmd y esperamos que se nos devuelve el valor de la columna que indica el IdRol del usuario
                    
                    if(result != null && result != DBNull.Value) //Si el procedure encontró un resultado
                    {
                        return Convert.ToInt32(result); //Se lo retorna, no sin antes convertirlo a INT, ya que se obtiene como type Object.
                    }
                }
            }
            return -1; //Si el procedure no obtuvo resultado, result será null, le damos un valor de -1, para asegurarnos que no funcione el objetivo mayor detras de esto (el acceso del usuario al formulario) pero que tampoco lanze alguna excepción.
        }

        //Query del procedimiento usado
        //create procedure ObtenerIdRol  
        //@NombreUsuario varchar(100)  
        //as begin  
        //select IdRol from Usuarios  
        //where NombreUsuario = @NombreUsuario  
        //end  



        //El otro metodo secundario que obtiene el otro argumento para el procedure del metodo principal. Copy and paste del otro.
        private static int ObtenerIdFormulario(string nombreFormulario)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {

                using(SqlCommand cmd = new SqlCommand("ObtenerIdFormulario", conexion))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                    cmd.Parameters.AddWithValue("@NombreFormulario", nombreFormulario); //Se argumenta el AbsolutePath sin la terminación, obtenida en la línea 26 de .Master.Cs

                    conexion.Open();

                    object result = cmd.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return -1;
        }

        //Query del procedimiento usado
        //create procedure ObtenerIdFormulario  
        //@NombreFormulario varchar(100)  
        //as begin  
        //select IdFormulario from Formularios  
        //where NombreFormulario = @NombreFormulario  
        //end  

    }
}