<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Usuarios_List.aspx.vb" Inherits="ManagerSenalesCaribe.Usuarios_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tbody>
            <tr height="100%">
                <td valign="top">
                    <table id="Table3" cellspacing="2" cellpadding="2" width="100%" border="0">
                        <tr>
                            <td>
                                <h2>
                                    &nbsp; &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblTitulo" runat="server" Text="Listado de Usuarios"></asp:Label></h2>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" CssClass="mensaje"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td style="width: 20%">
                                            Usuario:
                                        </td>
                                        <td style="width: 80%">
                                            <asp:TextBox CssClass="Textbox" ID="txtFiltro" runat="server" Width="80%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnBuscar" runat="server" CssClass="Data_Button" Text="Filtrar Usuarios">
                                            </asp:Button>
                                            <asp:Button ID="btnNuevo" runat="server" CssClass="Data_Button" Text="Agregar Nuevo Usuario">
                                            </asp:Button>
                                            <asp:Button ID="btnCancelar" runat="server" CssClass="Data_Button" Text="Regresar al Inicio"
                                                PostBackUrl="../Default.aspx" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h1>
                                    Usuarios Registrados</h1>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%">
                                <br />
                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" EmptyDataText="No existen usuarios que coincidan con su búsqueda"
                                    ForeColor="Black" GridLines="None" Width="100%" BackColor="White" BorderColor="#999999"
                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3">
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="Nombre de Usuario" />
                                        <asp:BoundField DataField="FullName" HeaderText="Descripción" />
                                        <asp:BoundField DataField="NextDatePassword_Change" DataFormatString="{0:dd/MM/yyyy}"
                                            HeaderText="Fecha Próximo Cambio" />
                                        <asp:CheckBoxField DataField="IsDeleted" HeaderText="Eliminado" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="cmdGridEdit" runat="server" AlternateText="Modificar Registro"
                                                    CommandArgument='<%# Eval("UserName") %>' CommandName="EditRecord"
                                                    ImageUrl="../images/edit.gif" />
                                                <asp:ImageButton ID="cmdGridDelete" runat="server" AlternateText="Eliminar Registro"
                                                    CommandArgument='<%# Eval("UserName") %>' CommandName="DeleteRecord"
                                                    ImageUrl="../images/delete.gif" OnClientClick="return confirm('Confirma que desea eliminar el usuario?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="Form_LabelField" HorizontalAlign="Left" />
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" CssClass="Textbox"
                                        HorizontalAlign="Left" />
                                    <AlternatingRowStyle BackColor="#ECEBE8" />
                                </asp:GridView>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
