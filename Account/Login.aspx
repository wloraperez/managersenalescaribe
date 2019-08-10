<%@ Page Title="Ingreso Admin" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="Login.aspx.vb" Inherits="ManagerSenalesCaribe.Login" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table id="Table1" cellspacing="1" cellpadding="1" width="80%" border="0" align="center">
        <tbody>
            <tr>
                <td valign="top" style="height: 16px">
                    <h2>
                        &nbsp;Login
                    </h2>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" CssClass="mensaje"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="2" border="0" align="center">
                        <tbody>
                            <tr bgcolor="Gainsboro">
                                <td colspan="4">
                                    <h1>
                                        Ingrese su Nombre de Usuario y Contraseña</h1>
                                </td>
                            </tr>
                            <tr bgcolor="WhiteSmoke">
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr bgcolor="WhiteSmoke">
                                <td valign="top" style="width: 20%" bgcolor="WhiteSmoke">
                                    Nombre de Usuario
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtUsuario" runat="server" Width="160px" MaxLength="12" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:RequiredFieldValidator ID="valUsuario" runat="server" ErrorMessage="*" Display="Dynamic"
                                        ControlToValidate="txtUsuario" Font-Size="8pt">Digite Su Nombre de Usuario</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr bgcolor="WhiteSmoke">
                                <td>
                                    Contraseña
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContrasena" AutoCompleteType="None" runat="server" Width="160px"
                                        TextMode="Password" Rows="12" CssClass="Textbox"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="valContrasena" runat="server" ErrorMessage="*" Display="Dynamic"
                                        ControlToValidate="txtContrasena" Font-Size="8pt">Digite Su Contraseña</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr bgcolor="WhiteSmoke">
                                <td align="right" width="115">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button CssClass="Data_Button" ID="btnLogin" runat="server" Text="Entrar" Width="104px">
                                    </asp:Button>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr bgcolor="WhiteSmoke">
                                <td colspan="4">
                                    <p>
                                    </p>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
