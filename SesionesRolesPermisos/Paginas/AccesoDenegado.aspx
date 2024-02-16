<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="SesionesRolesPermisos.Paginas.AccesoDenegado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Acceso Denegado</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous"/>
</head>
<body>
    <form id="form1" runat="server">
        <!--Creamos un div contenedor que dentró tendrá la tarjeta bootstrap con sus 2 secciónes header (tendrá el título) y el body (tendrá el texto) -->
        <div class="container text-center mt-4" style="width:34%;">
            <div class="card" style="border:2px solid black">
                <div class="card-header">
                    <h3 class="card-title"> Acceso Denegado</h3>
                </div>
                <div class="card-body">
                    <p class="card-text"> No tiene acceso a esta página.</p>
                </div>
            </div>
            <asp:Button runat="server" ID="btnVolver" CssClass="btn btn-outline-secondary mt-5" Text="Volver" OnClick="btnVolver_Click"/>
        </div>
    </form>
</body>
</html>
