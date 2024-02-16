<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="SesionesRolesPermisos.Paginas.IniciarSesion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Incio de Sesión</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous"/>
</head>
<body style="background-color:rgb(0,0,32)"> 

    <form id="form2" runat="server" class="mt-4">
        <!--Contenedor tarjeta que tendrá un encabezado (para que ocupe mas espacio el h4 dentro de el), y fuera del mismo sus controles asp.net que no estarán en el "card-body" ya que arruinaba el div, no se porque PREGUNTA.-->
        <div class="container card" style="width:33%; padding:10px 10px; border:6px solid; border-color:white; background-color:rgb(5,10,20);" >
            <div class="card-header">
                <h4 class="text-center mb-4" style="color:white; font-family:Bahnschrift">Inicio de sesión</h4>
            </div>
                <asp:Textbox runat="server" CssClass="form-control mb-3" ID="txtNombreUsuario" placeholder="Nombre de usuario.."></asp:Textbox>
                <asp:Textbox runat="server" CssClass="form-control mb-3" ID="txtContraseña" placeholder="*****" TextMode="Password" ></asp:Textbox>
                <asp:Button runat="server" ID="btnInciarSesion" CssClass="btn btn-dark border-light w-100" Text="Iniciar Sesion" OnClick="btnIniciarSesion_Click" />   
                <asp:Label runat="server" ID="lblMensajeError" Visible="false" BorderStyle="Dashed" style="margin-top:3%; width:40%; height:30px; background-color:white; font-family:'Trebuchet MS'; white-space:nowrap"></asp:Label>
                <asp:Button runat="server" ID="btnStopLabel" Visible="false" CssClass="btn btn-secondary" Text="Stop" OnClick="btnStopLabel_Click" style="margin-top:-7%; margin-left:70%; width: 25%;" />
        </div>
    </form>

    <script type="text/javascript">
        let lblMensajeError = document.getElementById('lblMensajeError'); //Asociamos el label mediante su ID a la variable que usaremos en este script.

        // Función que cambia el color de las letras.
        function cambiarColor() {
            let colores = ["red", "blue", "green"]; //Seleccionamos los 3 colores padres.
            let colorAleatorio = colores[Math.floor(Math.random() * colores.length)]; //Creamos una variable que obtendrá mediante índice un color aleatorio de la variable colores.

            lblMensajeError.style.color = colorAleatorio; //Le modificamos el color al label usando el color aleatorio obtenido

            //Posibilidades:  if Math.Random() => 0.67 return green == [2]; else if Math => 0.34 return blue == [1]; else return red == [0]
        }

        setInterval("cambiarColor()", 200); //La función cambiarColor() se ejecutará en bucle cada 30 centésimas. (300 milisegundos)
        //setInterval tiene como fin ejecutar cosas en bucles cumpliendo cierto intervalo de tiempo indicado en el 2do argumento.
        //En el primer argumento se indica que se repetirá en este caso al función que obtiene un color aleatorio
        //En el segundo argumento se indica cada cuantos milisegundos se repetirá. En este caso se usan 300 milisegundos, un 30% de un segundo, es decir 30 centésimas.


        //Lógica del color aleatorio.
        //Math.random() devuelve un numero decimal entre 0 y 0,999999... (nunca 1).
        //El Math.random() es multiplicado por 3. Lo que nos puede dar de resultado entre 0 && 2,999 (0 x 3 && 0,99999 x 3)
        //Usamos Math.floor para redondear el resultado a número entero y poder usarlo para acceder a un color del array colores.
        //Los posibles resultados finales serán 0, 1 y 2. Ya que si el producto es 2,999 se redondea a 2, si es 1,999 se redondea a 1, si es 0,999 se redondea a 0.
    </script>

</body>

</html>

            
        
    

    


