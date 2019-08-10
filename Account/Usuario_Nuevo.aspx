<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Usuario_Nuevo.aspx.vb" Inherits="ManagerSenalesCaribe.Usuario_Nuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table id="Table2" cellspacing="2" cellpadding="2" width="100%" border="0">
        <tr>
            <td colspan="2" class="top_header" height="16">
                <h2>
                    Nuevo Usuario</h2>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" CssClass="mensaje"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="Data_LabelText" colspan="2">
                <asp:Label ID="lblTitulo" runat="server" Visible="False"></asp:Label><br />
                <h1 class="OPTitulos">
                    Registro de Usuario - Datos Generales</h1>
            </td>
        </tr>
        <tr>
            <td style="width: 20%">
                Nombre de Usuario
            </td>
            <td style="width: 80%">
                <asp:TextBox ID="txtUserName" runat="server" MaxLength="12" Width="20%" CssClass="Textbox"></asp:TextBox>
                &nbsp;
                <asp:RequiredFieldValidator ID="valIDUsuario" runat="server" Font-Size="8pt" ControlToValidate="txtUserName"
                    Display="Dynamic" ErrorMessage="Por favor especifique el nombre de usuario"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Contraseña
            </td>
            <td>
                <asp:TextBox ID="txtUserPass" runat="server" MaxLength="14" Width="40%" TextMode="Password"
                    CssClass="Textbox"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="valContrasena" runat="server" Font-Size="8pt" ControlToValidate="txtUserPass"
                    Display="Dynamic" ErrorMessage="Digite la contraseña"></asp:RequiredFieldValidator>
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Repita la Contraseña
            </td>
            <td>
                <asp:TextBox ID="txtVerificacion" runat="server" MaxLength="14" Width="40%" TextMode="Password"
                    CssClass="Textbox"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="valVerificaContrasena" runat="server" Font-Size="8pt"
                    ControlToValidate="txtVerificacion" Display="Dynamic" ErrorMessage="Repita la contraseña"
                    Enabled="False"></asp:RequiredFieldValidator>&nbsp;<asp:CompareValidator ID="comValVerificacion"
                        runat="server" Font-Size="8pt" ControlToValidate="txtVerificacion" Display="Dynamic"
                        ErrorMessage="La Contraseña no concuerda con la anterior" ControlToCompare="txtUserPass"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>
                Nombre Completo
            </td>
            <td>
                <asp:TextBox ID="txtFullName" runat="server" Width="80%" CssClass="Textbox"></asp:TextBox>
                &nbsp;
                <asp:RequiredFieldValidator ID="valDescripcion" runat="server" Font-Size="8pt" ControlToValidate="txtFullName"
                    Display="Dynamic" ErrorMessage="Digite el Nombre Completo"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Correo Electrónico
            </td>
            <td class="Data_LabelText" height="21">
                <asp:TextBox ID="txtUserEmail" runat="server" Width="80%" CssClass="Textbox"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="valEmail" runat="server" Font-Size="8pt" ControlToValidate="txtUserEmail"
                    Display="Dynamic" ErrorMessage="Digite el Correo Electrónico"></asp:RequiredFieldValidator>
                &nbsp;<asp:RegularExpressionValidator ID="REVMail" runat="server" ControlToValidate="txtUserEmail"
                    ErrorMessage="RegularExpressionValidator" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Correo Electrónico 
                            inválido</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                Bloqueado?
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkIsLocked" />
            </td>
        </tr>
        <tr>
            <td>
                Administrador?
            </td>
            <td>
                <asp:CheckBox ID="chkIsAdmin" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Eliminado?
            </td>
            <td>
                <asp:CheckBox ID="chkIsDeleted" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="label">
                <asp:CheckBox ID="chkCambiar" runat="server" Text="Forzar a Cambiar Contraseña en el Próximo Ingreso"
                    Width="401px"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnGrabar" runat="server" CssClass="Data_Button" Text="Guardar Usuario">
                </asp:Button>
                &nbsp;<asp:Button ID="btnCancelar" runat="server" CssClass="Data_Button" Text="Regresar al Listado"
                    CausesValidation="False"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
