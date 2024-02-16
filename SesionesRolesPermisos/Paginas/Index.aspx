<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SesionesRolesPermisos.Paginas.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Menu principal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
        <!--Solo un label que dará la bienvenida al usuario logueado. Se le cambiará el Text en el code behind y se usará la variable de Sesión para mostrar el nombre adecuado.-->
                <asp:Label runat= "server" id="lblBienvenido" CssClass="display-4"  style="display:flex; position:absolute; left:10%; top:20%; font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;"></asp:Label>
</asp:Content>

