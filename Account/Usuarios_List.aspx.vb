Imports DataSenalesCaribe

Public Class Usuarios_List
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = String.Empty
        Me.lblMessage.CssClass = "mensaje"

        If Not Page.IsPostBack Then
            If Not IsNothing(Session("UserInfo")) Then
                If Not (CType(Session("UserInfo"), SeguridadClass.LoginClass).UserType = SeguridadClass.LoginClass.eUserLevel.Todos) Then
                    Response.Redirect("~/Default.aspx?msg=Acceso Denegado")
                End If
            Else
                Response.Redirect("~/Account/Login.aspx", False)
            End If

            Me.Process_Data()
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As System.EventArgs) Handles btnNuevo.Click
        Response.Redirect("Usuario_Nuevo.aspx", False)
    End Sub

    Private Sub Process_Data()
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = (From i In db.Usuarios Select i)

            If Me.txtFiltro.Text <> String.Empty Then
                oUsuarios = oUsuarios.Where(Function(d) d.FullName.Contains(Me.txtFiltro.Text) Or d.UserName.Contains(Me.txtFiltro.Text))
            End If

            If Not IsNothing(oUsuarios) AndAlso (oUsuarios.Count > 0) Then
                gv.DataSource = oUsuarios
                gv.DataBind()
            Else
                gv.DataSource = Nothing
                gv.DataBind()
            End If
        Catch ex As Exception
            Me.lblMessage.Text = ex.Message
            Me.lblMessage.CssClass += " error"
        End Try
    End Sub

    Private Sub gv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv.RowCommand
        Me.lblMessage.Text = String.Empty
        Me.lblMessage.CssClass = "mensaje"

        Select Case e.CommandName

            Case "EditRecord"
                Response.Redirect("Usuario_Nuevo.aspx?idUsuario=" & e.CommandArgument.ToString())
            Case "DeleteRecord"
                Try
                    Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

                    Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = e.CommandArgument.ToString())

                    If Not IsNothing(oUsuarios) Then
                        oUsuarios.IsDeleted = True
                        db.SubmitChanges()

                        Me.lblMessage.Text = "Usuario eliminado con exito."
                        Me.lblMessage.CssClass += " success"
                        Me.Process_Data()
                    End If
                Catch ex As Exception
                    Me.lblMessage.Text = ex.Message
                    Me.lblMessage.CssClass += " error"
                End Try

                'Case "Disconnect"
                '    ForceDestroyLoginCache(e.CommandArgument)

                '    Process_Data()
        End Select
    End Sub

End Class