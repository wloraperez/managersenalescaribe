Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblMessage.Text = String.Empty
        Me.lblMessage.CssClass = "mensaje"

        If Not Page.IsPostBack Then
            If IsNothing(Session("UserInfo")) Then
                Response.Redirect("~/Account/Login.aspx", False)
                Return
            End If

            If Not IsNothing(Request("msg")) Then
                Me.lblMessage.Text = Request("msg")
                Me.lblMessage.CssClass += " error"
            End If
        End If
    End Sub

End Class