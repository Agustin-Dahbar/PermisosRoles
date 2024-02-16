<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="CrearFormularios.aspx.cs" Inherits="SesionesRolesPermisos.CrearFormularios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Creacion de Formularios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%-- El contenido de a continuación se ubicará en la línea 41 de la página maestra, ya que allí está ubicando el ContentPlaceHolder con el ID de Body. --%>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">    
    <div class="container mt-4" style="width:50%"> <!-- div que será la estructura, dada por la clase con fines de maquetación, container-->
      <div class="card"> <!--Creamos una tarjeta bootstrap-->
            <div class="card-header"> <!--Encabezado de la tarjeta-->
                <h3 class="card-title"> Crear nuevo formulario</h3> <!--Título de la tarjeta-->
            </div>
            <div class="card-body"> <!--Cuerpo de la tarjeta-->
                <asp:TextBox runat="server" ID="txtRutaFormulario" placeholder="Ruta Formulario" CssClass="form-control mb-3"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtNombreFormulario" placeholder="NombreFormulario" CssClass="form-control mb-3"></asp:TextBox>
                <asp:Button runat="server" ID="btnCrearFormulario" Text="Registrar Formulario" CssClass="btn btn-primary" OnClick="btnCrearFormulario_Click"/>
            </div>
            <div class="card-footer"> <!--Footer de la tarjeta-->
                <asp:Label runat="server" ID="lblMensaje" CssClass="text-success"></asp:Label> <!--Mensaje que se mostrará indicando error o éxito. En caso de error, se cambía dinámicaente el texto y el CssClass del label para adaptarse al error lanzado. -->
            </div>
      </div>
    </div> 
</asp:Content>

