using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SesionesRolesPermisos
{
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            try
            {
                string cadenaDeConexion = ConfigurationManager.ConnectionStrings["conexion"].ToString();
                SqlConnection conexion = new SqlConnection(cadenaDeConexion);
                return conexion;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }



    //Para ahorrar líneas de código podemos directamente argumentar al objeto SqlConnection con la ruta de acceso a la cadena de conexión
    // SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ToString();
    //Lo mantenemos con más detalle por ser más explicativo, pedagogico. De todas formas, último proyecto en el que se hace asi.
}