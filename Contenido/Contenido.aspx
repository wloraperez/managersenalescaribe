<%@ Page Title="Contenido" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    MaintainScrollPositionOnPostback="true" CodeBehind="Contenido.aspx.vb" Inherits="ManagerSenalesCaribe.Contenido"
    ValidateRequest="false" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2010" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%--    <link rel="stylesheet" type="text/css" href="../css/jquery-ui/smoothness/jquery-ui-1.7.2.custom.css" />
    <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
    <script src="../js/jquery.flash.js" type="text/javascript"></script>
    <script src="../js/agilisa_media.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.7.2.custom.min.js"></script>--%>
    <link href="../Styles/smoothness/jquery-ui-1.10.1.custom.min.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jquery-ui-1.10.1.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    <style type="text/css">
        .gallery-item
        {
            float: left;
            list-style: none outside none;
            margin: 0;
            padding: 5px;
            position: relative;
        }
        
        .PhotoFormTrue, .PhotoFormFalse
        {
            padding: 4px;
            vertical-align: top;
            margin: 10px 10px 30px 10px;
            border: solid 1px gray;
            display: inline-block;
        }
        .PhotoFormFalse
        {
            background-color: #ff4;
        }
        .PhotoFormTrue
        {
            background-color: #f44;
        }
        #photocmd
        {
            text-align: center;
            float: right;
            width: 1px;
            white-space: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="ErrorTag">
        <asp:Label runat="server" ID="lblMsg"></asp:Label></div>
    <h2>
        Administracion de Contenido</h2>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <h3>
                Acceso denegado</h3>
            <p>
                Este usuario no tiene permiso a acceder a este modulo. Si considera que es un error
                , favor comunicarse con el administrador.</p>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table>
                <tr>
                    <td>
                        <h3>
                            Filtrar</h3>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="Highlight_Option_Gray">
                                    Buscar:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchBox" runat="server" Width="350px" CssClass="Textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="Highlight_Option_Gray">
                                    Categoria:
                                </td>
                                <td>
                                    <asp:DropDownList ID="lstCategoria" CssClass="Textbox" DataTextField="CatName" DataValueField="IDCategory"
                                        Width="375px" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="Data_Button" />&nbsp;
                        <asp:Button ID="cmdLimpiar" runat="server" Text="Limpiar" CssClass="Data_Button" />&nbsp;
                        <asp:Button ID="cmdAddNew" runat="server" Text="Nuevo Articulo" CssClass="Data_Button" />
                    </td>
                </tr>
            </table>
            <hr />
            <div>
                <asp:GridView ID="gvContenido" runat="server" AutoGenerateColumns="False" EmptyDataText="No existen registros que coincidan con su seleccion"
                    ForeColor="Black" GridLines="None" Width="100%" BackColor="White" BorderColor="#999999"
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3">
                    <Columns>
                        <asp:BoundField DataField="PublishDate" HeaderText="Fecha" DataFormatString="{0:yyyy/MMM/dd}" />
                        <asp:BoundField DataField="Title" HeaderText="Titulo" />
                        <asp:TemplateField ItemStyle-Width="1px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Edit.gif" CommandName="Editar"
                                    CommandArgument='<%# Eval("IDContent") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="1px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/Delete.gif"
                                    CommandName="Borrar" CommandArgument='<%# Eval("IDContent") %>' OnClientClick="javascript: return confirm('Esta seguro de eliminar este contenido?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="Form_LabelField" HorizontalAlign="Left" />
                    <FooterStyle BackColor="#CCCCCC" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" CssClass="Highlight_Option_Gray"
                        HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="#ECEBE8" />
                </asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <h3>
                Agregar/Editar Contenido</h3>
            <table width="100%" style="background-color: White;">
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
                        Fecha Publicaci&oacute;n:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditPublishDate" runat="server" CssClass="Textbox" Width="70%"></asp:TextBox>
                        <rjs:PopCalendar ID="PopCalPublishDate" runat="server" Control="txtEditPublishDate"
                            Culture="es-DO" />
                    </td>
                    <td class="Highlight_Option_Gray">
                        Fecha Expiraci&oacute;n:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEditExpireDate" runat="server" CssClass="Textbox" Width="70%"></asp:TextBox>
                        <rjs:PopCalendar ID="PopCalExpiredDate" runat="server" Control="txtEditExpireDate" />
                    </td>
                </tr>
                <tr>
                    <td class="Highlight_Option_Gray">
                        Titulo:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEditTitulo" runat="server" CssClass="Textbox" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Highlight_Option_Gray">
                        Orden:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEditOrderBy" runat="server" CssClass="Textbox" Width="20%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Highlight_Option_Gray">
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="cbIsPublished" runat="server" Text="Publicado" />
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="Data_Button" />
                <asp:Button ID="cmdGoBack" runat="server" Text="Cancelar" CssClass="Data_Button" />
            </div>
            <div id="tabs" style="min-height: 200px; width: 100%; display: inline-block; margin-top: 10px;">
                <ul>
                    <li><a href="#tab-content">Contenido</a></li>
                    <li><a href="#tab-images">Imagenes</a></li>
                </ul>
                <div>
                    <div id="tab-content">
                        <fieldset>
                            <legend class="Highlight_Option_Gray">Contenido</legend>
                            <FTB:FreeTextBox ID="freeEditorContenido" runat="server" Height="200px" Width="90%"
                                ToolbarLayout="RemoveFormat | Bold, Italic, Underline, Strikethrough | CreateLink, Unlink | JustifyLeft, JustifyRight, JustifyCenter, JustifyFull ; BulletedList, NumberedList | Indent, Outdent | Cut, Copy, Paste | Undo, Redo ; Print, Save">
                            </FTB:FreeTextBox>
                        </fieldset>
                        <fieldset>
                            <legend class="Highlight_Option_Gray">Categorias</legend>
                            <asp:CheckBoxList ID="cblEditCategories" runat="server" RepeatColumns="3" CssClass="Textbox">
                            </asp:CheckBoxList>
                        </fieldset>
                    </div>
                    <div id="tab-images">
                        <h3>
                            Agregar Imagenes
                        </h3>
                        <table width="100%">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="Highlight_Option_Gray">
                                                Imagen:
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="Textbox" />
                                            </td>
                                            <td>
                                                <asp:Button ID="cmdUploadImage" runat="server" Text="Subir Imagen" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Comentario de la Imagen"></asp:Label>
                                    <asp:TextBox ID="txtEditPhotoComent" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView runat="server" ID="lvImages">
                                        <ItemTemplate>
                                            <li class="gallery-item">
                                                <%--<div class="PhotoForm<%# (Eval("IDPhoto") = IDPhotoDefault) %>">--%>
                                                <div class="PhotoFormTrue">
                                                    <%# ManagerSenalesCaribe.ImagenesUtil.getImagetag(Eval("IDPhoto"), Eval("ContentType"), Eval("Description"), false)%>
                                                    <div id="photocmd">
                                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Edit.gif" CommandName="Editar"
                                                            CommandArgument='<%# Eval("IDPhoto") %>' />--%>
                                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/Delete.gif"
                                                            CommandName="Borrar" CommandArgument='<%# Eval("IDPhoto") %>' OnClientClick="javascript: return confirm('Confirma que desea eliminar este elemento?');" />
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                        <LayoutTemplate>
                                            <ul id="galeria">
                                                <li runat="server" id="itemPlaceholder" />
                                            </ul>
                                            <div class="clear">
                                            </div>
                                        </LayoutTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
