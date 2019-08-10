<%@ Page Title="Secciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Categories.aspx.vb" Inherits="ManagerSenalesCaribe.Categories" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%--    <link rel="stylesheet" type="text/css" href="../css/jquery-ui/redmond/jquery-ui-1.7.2.custom.css" />
    <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.7.2.custom.min.js"></script>--%>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .SelectItem
        {
            background-color: Yellow;
        }
        .CatItem
        {
            font-size: 1.1em;
            font-family: Verdana, Tahoma, Arial;
        }
        
        .panes
        {
            margin: 10px;
            font-size: 1.2em;
            border: 1px solid #e1e1e1;
            padding: 5px;
            width: 100%;
        }
        .panes td
        {
            padding: 5px;
        }
        
        caption
        {
            background-color: #F2F2F2;
            font-size: 1.2em;
            font-weight: bold;
            margin-bottom: 5px;
            padding: 5px;
            text-align: left;
            width: 100%;
        }
        
        .field
        {
            min-width: 80px;
            background-color: #eee;
            font-size: 1em;
            font-family: Verdana, helvetica, sans-serif;
            text-align: right;
        }
    </style>
    <%--<link type="text/css" href="../css/buttons.css" rel="stylesheet" />--%>
    <!-- Button JavaScript -->
    <%--    <script type="text/javascript" src="../js/SimplyButtons.js"></script>
    <script type="text/javascript">
        $(function () {
            SimplyButtons.init();

            //$("#tabs").tabs();
        });
    </script>--%>
    <!--PNG FIX-->
    <!--[if IE 6]>
	  <script type="text/javascript" src="js/unitpngfix.js"></script>
	<![endif]-->
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 100%">
        <h2>
            Secciones de la página web</h2>
        <div style="width: 300px; float: left;">
            <asp:TreeView ID="tvCat" runat="server" NodeStyle-CssClass="CatItem">
                <SelectedNodeStyle CssClass="SelectItem" />
                <NodeStyle CssClass="CatItem"></NodeStyle>
            </asp:TreeView>
        </div>
        <div style="width: 600px; float: right;">
            <div style="margin: 10px; padding: 5px;">
                <table width="100%">
                    <tr>
                        <td class="Highlight_Option_Gray">
                            ID:
                        </td>
                        <td>
                            <asp:Label ID="lblID" runat="server" Text="-1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Ruta:
                        </td>
                        <td>
                            <asp:Label ID="lblEditCatCategory" runat="server" Text="[raiz]"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Depende de:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlParent" CssClass="Textbox" runat="server" DataTextField="CatName"
                                DataValueField="IDCategory">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Tipo:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEditTipoCategory" runat="server" CssClass="Textbox" Width="98%"
                                DataTextField="Descripcion" DataValueField="CustomInt1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Nombre:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditCatName" runat="server" CssClass="Textbox" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Descripcion:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditCatDescripcion" runat="server" CssClass="Textbox" Width="98%"
                                TextMode="MultiLine" Rows="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            URL:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditCatURL" runat="server" CssClass="Textbox" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                            Orden:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEditCatOrderBy" runat="server" CssClass="Textbox" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Highlight_Option_Gray">
                        </td>
                        <td class="CheckText">
                            <asp:CheckBox ID="cbEditIsMenu" runat="server" Text="Es Menu?" /><br />
                        </td>
                    </tr>
                </table>
                <div style="margin-top: 10px;">
                    <asp:LinkButton ID="lnkGuardar" runat="server" CssClass="Data_Button"><span><span>Guardar</span></span></asp:LinkButton>
                    <asp:LinkButton ID="lnkCmdGoBack" runat="server" CssClass="Data_Button"><span><span>Cancelar</span></span></asp:LinkButton>
                    <asp:LinkButton ID="lnkCmdAddChild" runat="server" CssClass="Data_Button"><span><span>Agregar Hijo</span></span></asp:LinkButton>
                    <asp:LinkButton ID="lnkCmdDelete" runat="server" CssClass="Data_Button"><span><span>Eliminar</span></span></asp:LinkButton>
                </div>
            </div>
            <div>
                <fieldset>
                    <legend>Subir Archivo</legend>
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="Textbox" />
                    <asp:Button ID="cmdDeleteImage" Text="Eliminar Imagen" runat="server" />
                </fieldset>
                <div style="margin: 5px;">
                    <asp:Image ID="imgDefault" ImageUrl="~/vImg/Imagenes.aspx?ID={0}" runat="server"
                        Width="95%" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
