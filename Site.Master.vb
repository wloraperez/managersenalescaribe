Imports System.Globalization
Imports System.Threading
Imports DataSenalesCaribe

Public Class Site
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-DO")

        If Not Page.IsPostBack Then
            'Me.panelLogin.Visible = True
            If My.User.IsAuthenticated And Not IsNothing(Session("UserInfo")) Then
                Session.Add("Usuario", CType(Session("UserInfo"), SeguridadClass.LoginClass).FullName)

                Me.cmdCurrentUser.Text = Session("Usuario").ToString()
                Me.cmdLogout.Attributes.Add("onClick", "javascript:return confirm('Desea Cerrar la Sesion y Salir de la Aplicacion ?');")

                Me.panelLogin.Visible = True
            Else
                Me.panelLogin.Visible = False
            End If
        End If
    End Sub

    Protected Sub cmdLogout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdLogout.Click
        Session.Abandon()
        Session.RemoveAll()
        FormsAuthentication.SignOut()
        Response.Redirect("~/Account/login.aspx")
    End Sub

End Class