Imports DataSenalesCaribe

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = String.Empty
        Me.lblMessage.CssClass = "mensaje"
        Me.txtUsuario.Focus()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Me.lblMessage.Text = String.Empty
        Me.lblMessage.CssClass = "mensaje"

        Dim _respuesta As Short
        Dim UserInfo As New SeguridadClass.LoginClass
        Try

            _respuesta = ValidaContrasena(txtContrasena.Text, txtUsuario.Text)
            If _respuesta = eRespuestaLogin.Exitoso Then
                If FechaContrasenaVencida(txtUsuario.Text) Then
                    FormsAuthentication.SetAuthCookie(txtUsuario.Text, False)  'Para que no haga login automático

                    Response.Redirect("~/Account/ChangePassword.aspx?Msg=Su clave ha expirado. Por favor cambie su clave inmediatamente.")
                Else
                    FormsAuthentication.SetAuthCookie(txtUsuario.Text, False)
                    Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)
                    Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = Me.txtUsuario.Text)

                    If Not IsNothing(oUsuarios) Then
                        UserInfo.FullName = oUsuarios.FullName
                    Else
                        oUsuarios.FullName = ""
                    End If
                    'Guardando la estructura del usuario
                    UserInfo.NameUser = Me.txtUsuario.Text
                    UserInfo.CheckUserEventsLevel()
                    Session("UserInfo") = UserInfo

                    'Response.Redirect("../Default.aspx")
                    FormsAuthentication.RedirectFromLoginPage(Me.txtUsuario.Text, False)
                End If
            ElseIf _respuesta = eRespuestaLogin.Denegado Then
                lblMessage.Visible = True
                lblMessage.Text = "El usuario y clave provisto son invalidos. Por favor intente de nuevo..."
            ElseIf _respuesta = eRespuestaLogin.Bloqueado Then
                lblMessage.Visible = True
                lblMessage.Text = "Su cuenta ha sido temporalmente bloqueada por razones de seguridad. Por favor contacte el administrador del sistema."
            End If
        Catch ex As Exception
            lblMessage.CssClass += " error"
            lblMessage.Text = ex.Message
        End Try
    End Sub

End Class