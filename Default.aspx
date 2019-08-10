<%@ Page Title="Administrador Señales del Caribe" Language="vb" MasterPageFile="~/Site.Master"
    AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ManagerSenalesCaribe._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table id="Table2" cellspacing="2" cellpadding="2" width="100%" border="0">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" CssClass="mensaje"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
