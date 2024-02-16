<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="AdministrarPermisos.aspx.cs" Inherits="SesionesRolesPermisos.Paginas.AdministrarPermisos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container pb-3 pt-3"> <!--Como mover controladores Repeaters GridViews que no acepten style o CssClass. La única manera es moverlos con flexbox desde acá pero no se puede hacer mucho. -->
        <h1 class="mb-4" style="display:flex; position:absolute; left:2%;">Administrador de Permisos</h1>
        <div style="height:80px;"></div>
        <!--Repeater principal (repetirá la columna NombreRol de la tabla Roles). Para eso se le dió a su propiedad DataSource el resultado de la query select * from Roles, es decir, se le dió como fuente de origen de datos los datos de la tabla Roles entera.-->
        <asp:Repeater runat="server" ID="rptRoles" OnItemDataBound="rptRoles_ItemDataBound">
            <ItemTemplate> <!--Es la fila, que contendrá al controlador que será el input de salida de los Nombres de Roles, será un h3.-->
                <div class="card mb-3"> <!--Tarjeta estilizada Bootstrap-->
                    <div class="card-header"> <!--Encabezado de la tarjeta-->
                        <h3 class="card-title" runat="server">Permisos del <%#Eval("NombreRol") %></h3> <!--Título en el encabezado, obtendrá los valores de la columna NombreRol mediante el Eval().-->
                    </div>
                    <div class="card-body"> <!--Cuerpo de la tarjeta-->
                        <ul class="list-group"> <!--Unorder list que almacenará al repeater de formularios, que repetirá a los ListItem.-->      
                            <asp:Repeater runat="server" ID="rptFormularios"> <%--Repeater secundario, repetira al ListItem que mostrará el conjunto de controles que serán los inputs de salida de las columnas específicadas en los Eval() de la tabla Formulario. Se le dió origen de datos la tabla Formularios. En el evento del rptRoles_ItemDataBound--%>
                                <ItemTemplate> <!--Item del rpt-->
                                    <li class="list-group-item d-flex justify-content-between align-items-center"> <!--ListItem usamos la clase bootstrap que lo compatibilizará con la UL que usa la misma clase contenedora.-->
                                        <label><%#Eval("NombreFormulario") %></label> <!--Label que mostrara los valores de la columna "NombreFormulario"-->
                                        <asp:HiddenField runat="server" ID="hdnIdFormulario" Value='<%#Eval("IdFormulario") %>'/> <!--HiddenField, nos sirve para obtener un valor que usaremos como argumento en el metodo que obtiene el valor de la columna Permisos de la tabla Permisos para dárselo a la propiedad Checked del checkbox y así la repetición del ItemTemplate sepa si debe o no checkear el checkbox. Por eso lo escondemos, no tiene un fin visual.-->
                                        <asp:CheckBox runat="server" ID="chkPermisos" CssClass="form-check-input"/> <!--Checkbox con el que podremos cambiar los permisos cambiando su propiedad Checked clickeando en el.-->
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="card-footer"> <!--Footer de la carta, tendrá un label invisible.-->
                        <asp:Label runat="server" ID="lblIdRol" Visible="false" Text='<%#Eval("IdRol") %>'></asp:Label> <!--Label que obtendrá el IdRol de la tabla Roles, el otro argumento que necesitaremos para el caso mencionado en el HiddenField. Es decir que este label tiene el mismo fin que el HiddenField pero cada uno obtuvo un argumento.-->
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <!--Fin del repeater principal-->
        <asp:Button runat="server" CssClass="btn btn-dark" ID="btnGuardarPermisos" Text="Actualizar permisos" OnClick="btnGuardarPermisos_Click"/>
        <asp:Label runat="server" ID="lblMensaje" CssClass="mt-3"></asp:Label>
    </div>
</asp:Content>
