Imports System.Security.Cryptography
Imports DataSenalesCaribe

Public Class Usuario_Nuevo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not (CType(Session("UserInfo"), SeguridadClass.LoginClass).UserType = SeguridadClass.LoginClass.eUserLevel.Todos) Then
                Response.Redirect("~/Default.aspx?msg=Acceso Denegado")
            End If

            If Not Request("idUsuario") = String.Empty Then
                If FillUsuario(Request("idUsuario")) Then
                    btnGrabar.Text = "Actualizar Usuario"
                    lblTitulo.Text = "Modificación de Usuario"
                    valContrasena.Enabled = False
                    valVerificaContrasena.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Function FillUsuario(ByVal idUsuario As String) As Boolean
        Me.lblMessage.Text = ""
        Me.lblMessage.CssClass = "mensaje"
        Dim _oret As Boolean = False
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = idUsuario)

            If Not IsNothing(oUsuarios) Then
                With oUsuarios
                    Me.txtUserName.Text = idUsuario
                    Me.txtUserName.ReadOnly = True
                    Me.txtUserPass.Text = .UserPass
                    Me.txtVerificacion.Text = .UserPass
                    Me.txtFullName.Text = .FullName
                    Me.txtUserEmail.Text = .UserEmail
                    Me.chkIsLocked.Checked = (IIf(Not IsNothing(.IsLocked), .IsLocked, True))
                    Me.chkIsAdmin.Checked = (IIf(Not IsNothing(.IsAdmin), .IsAdmin, False))
                    Me.chkIsDeleted.Checked = (IIf(Not IsNothing(.IsDeleted), .IsDeleted, True))
                    _oret = True
                End With
            Else
                Me.lblMessage.Text = "Usuario no encontrado"
                Me.lblMessage.CssClass += " error"
                _oret = False
            End If

        Catch ex As Exception
            Me.lblMessage.Text = ex.Message
            Me.lblMessage.CssClass += " error"
        End Try

        Return _oret
    End Function

    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click
        Me.lblMessage.Text = ""
        Me.lblMessage.CssClass = "mensaje"

        If ValidatePass() Then
            If ValidateUser() Then

                If btnGrabar.Text = "Guardar Usuario" Then
                    If UsuarioExiste(txtUserName.Text) Then
                        Me.lblMessage.Text = "Este nombre de usuario ya existe."
                        Me.lblMessage.CssClass += " info"
                    Else
                        If UsuariosAdd() Then
                            Me.lblMessage.Text = "Registro Adicionado"
                            Me.lblMessage.CssClass += " success"
                            Me.btnGrabar.Visible = False
                            Me.btnCancelar.Text = "Regresar"
                        End If
                    End If
                ElseIf btnGrabar.Text = "Actualizar Usuario" Then
                    If UsuariosModifica() Then
                        Me.lblMessage.Text = "Registro Actualizado"
                        Me.lblMessage.CssClass += " success"
                        Me.btnGrabar.Visible = False
                        Me.btnCancelar.Text = "Regresar"
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("Usuarios_List.aspx")
    End Sub

    Private Function ValidatePass() As Boolean
        Dim InvalidContrasena As String = String.Empty
        Dim IsError As Boolean = False

        If Me.txtUserPass.Text <> "" Then
            Dim _longMin = Comun.GetParametroInt1("UserPass_LongitudMinima")
            Dim _longMax = Comun.GetParametroInt1("UserPass_LongitudMaxima")

            If (Me.txtUserPass.Text.Length < _longMin Or _longMin = -1) OrElse (Me.txtUserPass.Text.Length > _longMax Or _longMax = -1) Then
                InvalidContrasena += String.Format("La Contraseña debe contener mínimo {0} caracteres y máximo {1} caracteres", _longMin, _longMax)
                IsError = True
            End If
        Else
            InvalidContrasena += " Debe escribir una contraseña."
            IsError = True
        End If

        If IsError Then
            Me.lblMessage.Text = InvalidContrasena
            Me.lblMessage.CssClass += " error"
        End If

        Return Not IsError
    End Function

    Private Function ValidateUser() As Boolean
        Dim InvalidUser As String = String.Empty
        Dim IsError As Boolean = False

        If Me.txtUserName.Text <> "" Then
            Dim _longMin = Comun.GetParametroInt1("UserName_LongitudMinima")
            Dim _longMax = Comun.GetParametroInt1("UserName_LongitudMaxima")

            If (Me.txtUserName.Text.Length < _longMin Or _longMin = -1) OrElse (Me.txtUserName.Text.Length > _longMax Or _longMax = -1) Then
                InvalidUser += String.Format("El usuario Debe contener mínimo {0} caracteres y máximo {1} caracteres", _longMin, _longMax)
                IsError = True
            End If
        Else
            InvalidUser += " Debe escribir un nombre de usuario."
            IsError = True
        End If

        If IsError Then
            lblMessage.Text = InvalidUser
            Me.lblMessage.CssClass += " info"
        End If

        Return Not IsError
    End Function

    Private Function UsuariosAdd() As Boolean
        Dim _oret As Boolean = False

        Dim _diasCambioContrasena = Comun.GetParametroInt1("Seguridad_DiasCambioContrasena")

        If chkCambiar.Checked Then 'Para forzar a cambiar la contraseña del usuario en el primer login
            _diasCambioContrasena = -1
        End If
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios As New DataSenalesCaribe.Usuario

            With oUsuarios
                .UserName = Me.txtUserName.Text
                .FullName = Me.txtFullName.Text

                .UserPass = EncryptContrasena(Me.txtUserPass.Text)
                .NextDatePassword_Change = System.DateTime.Now.AddDays(_diasCambioContrasena)
                .IsLocked = Me.chkIsLocked.Checked
                .UserEmail = Me.txtUserEmail.Text
                .IsAdmin = Me.chkIsAdmin.Checked
                .Create_Date = Now
                .Create_User = User.Identity.Name
                .Update_Date = Now
                .Update_User = User.Identity.Name
                .IsDeleted = Me.chkIsDeleted.Checked

                db.Usuarios.InsertOnSubmit(oUsuarios)
                db.SubmitChanges()

                _oret = True
            End With
        Catch ex As Exception
            _oret = False
            Me.lblMessage.Text = ex.Message
            Me.lblMessage.CssClass += " error"
        End Try

        Return _oret
    End Function

    Public Function UsuariosModifica() As Boolean
        Dim _oret As Boolean

        Dim _hashData As [Byte]() = ConvertStringToByteArray(txtUserPass.Text)
        Dim _hashValue As Byte() = CType(CryptoConfig.CreateFromName("MD5"), HashAlgorithm).ComputeHash(_hashData)

        Dim _diasCambioContrasena As Integer = Comun.GetParametroInt1("Seguridad_DiasCambioContrasena")

        If _diasCambioContrasena = -1 Then Me.lblMessage.Text = "Atención: El parámetro Seguridad_DiasCambioContrasena no fue encontrado. "

        If chkCambiar.Checked Then 'Para forzar a cambiar la contraseña del usuario en el primer login
            _diasCambioContrasena = -1
        End If
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = Me.txtUserName.Text)

            If Not IsNothing(oUsuarios) Then
                With oUsuarios
                    .FullName = Me.txtFullName.Text

                    If Not Me.txtUserPass.Text = String.Empty Then
                        .UserPass = BitConverter.ToString(_hashValue)
                    End If
                    .NextDatePassword_Change = System.DateTime.Now.AddDays(_diasCambioContrasena)
                    .IsLocked = Me.chkIsLocked.Checked
                    .UserEmail = Me.txtUserEmail.Text
                    .IsAdmin = chkIsAdmin.Checked
                    .IsDeleted = Me.chkIsDeleted.Checked
                    .Update_Date = Now
                    .Update_User = User.Identity.Name

                    db.SubmitChanges()

                    _oret = True
                End With
            Else
                Me.lblMessage.Text += "El nombre de usuario no ha sido encontrado."
                Me.lblMessage.CssClass += " info"
                _oret = False
            End If

        Catch ex As Exception
            _oret = False
            Me.lblMessage.Text += ex.Message
            Me.lblMessage.CssClass += " error"
        End Try

        Return _oret
    End Function

    Public Function UsuarioExiste(ByVal idUsuario As String) As Boolean
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = idUsuario)
            If Not IsNothing(oUsuarios) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Me.lblMessage.Text = ex.Message
            Me.lblMessage.CssClass += " error"
        End Try

        Return False
    End Function
End Class