Imports System
Imports System.Web.SessionState
Imports System.Web.Security
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports System.Configuration
Imports System.Collections
Imports DataSenalesCaribe
Imports System.Data
Imports System.Web.HttpContext
Imports System.Web

Public Module SeguridadClass

    Public Class LoginClass
        Private _NameUser As String
        Private _fullName As String
        Private _UserType As eUserLevel = eUserLevel.None

        Public Property NameUser() As String
            Get
                Return _NameUser
            End Get
            Set(ByVal value As String)
                _NameUser = value
            End Set
        End Property

        Public Property FullName() As String
            Get
                Return _fullName
            End Get
            Set(ByVal value As String)
                _fullName = value
            End Set
        End Property

        Public Property UserType() As Integer
            Get
                Return _UserType
            End Get
            Set(ByVal value As Integer)
                _UserType = value
            End Set
        End Property

        Public Enum eUserLevel As Integer
            Todos = 0
            Invitado = 1
            None = 3
        End Enum

        Public Sub CheckUserEventsLevel()
            Try
                Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

                Dim _Usuario = db.Usuarios.FirstOrDefault(Function(d) d.UserName = NameUser)

                If Not IsNothing(_Usuario) Then
                    If _Usuario.IsAdmin Then
                        UserType = eUserLevel.Todos
                    Else
                        UserType = eUserLevel.Invitado
                    End If
                Else
                    UserType = eUserLevel.None
                End If
            Catch ex As Exception
                UserType = eUserLevel.None
            End Try
        End Sub
    End Class

    Public Enum eRespuestaLogin As Short
        Exitoso = 1
        Denegado = 2
        Bloqueado = 3
    End Enum

    Public Function ConvertStringToByteArray(ByVal s As [String]) As [Byte]()
        Return (New UnicodeEncoding).GetBytes(s)
    End Function

    Public Function bloqueaUsuario(ByVal _usuario As String) As Boolean
        Dim _oret As Boolean

        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = _usuario)

            If Not IsNothing(oUsuarios) Then
                oUsuarios.IsLocked = True
                oUsuarios.Expired_Date = Now
                db.SubmitChanges()
                _oret = True
            Else
                _oret = False
            End If
        Catch ex As Exception
            _oret = False
        End Try

        Return _oret
    End Function

    Public Function EncryptContrasena(ByVal _contrasena As String) As String
        Dim _hashData As [Byte]() = ConvertStringToByteArray(_contrasena)
        Dim _hashValue As Byte() = New MD5CryptoServiceProvider().ComputeHash(_hashData)

        Return BitConverter.ToString(_hashValue)
    End Function

    Public Function ValidaContrasena(ByVal _contrasena As String, ByVal _usuario As String) As Short
        Dim _oret As Short

        Dim _intentos As Integer
        Dim _Expired_Date As DateTime
        Dim _IntentosFallidos As Integer = Comun.GetParametroInt1("IntentosFallidosMax")
        Dim _IntervaloBloqueo As Integer = Comun.GetParametroInt1("IntervaloBloqueoMin")

        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = _usuario And d.IsDeleted = False)

            If Not IsNothing(oUsuarios) Then
                With oUsuarios
                    If Not IsNothing(.Intentos) Then _intentos = .Intentos

                    If Not IsNothing(.Expired_Date) Then
                        _Expired_Date = .Expired_Date
                    Else
                        _Expired_Date = Now
                    End If

                    If .IsLocked AndAlso DateDiff(DateInterval.Minute, _Expired_Date, Now) > _IntervaloBloqueo Then
                        .IsLocked = False
                        .Intentos = 0
                        db.SubmitChanges()
                    End If

                    If .IsLocked = False Then
                        If .UserPass = EncryptContrasena(_contrasena) Then
                            .Intentos = 0
                            db.SubmitChanges()
                            _oret = eRespuestaLogin.Exitoso
                        Else
                            _intentos += 1
                            If _intentos < _IntentosFallidos Then
                                .Intentos = _intentos
                                db.SubmitChanges()
                            Else
                                .IsLocked = True
                                .Intentos = 0
                                .Expired_Date = Now
                                db.SubmitChanges()
                                _oret = eRespuestaLogin.Bloqueado
                            End If
                        End If
                    Else
                        _oret = eRespuestaLogin.Bloqueado
                    End If

                End With
            Else
                _oret = eRespuestaLogin.Denegado
            End If
        Catch ex As Exception
            _oret = eRespuestaLogin.Denegado
        End Try

        Return _oret
    End Function

    Public Function FechaContrasenaVencida(ByVal _usuario As String) As Boolean
        Dim _oret As Boolean

        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim oUsuarios = db.Usuarios.FirstOrDefault(Function(d) d.UserName = _usuario)

            If Not IsNothing(oUsuarios) Then
                _oret = oUsuarios.NextDatePassword_Change <= DateTime.Now
            Else
                _oret = False
            End If
        Catch ex As Exception
            _oret = False
        End Try

        Return _oret
    End Function

#Region "Cache Manager"

    'Public Enum LoginCacheResult
    '    Matched
    '    NotMatched
    '    Created
    '    Disconnect
    'End Enum

    'Public Function CheckConcurrency(ByVal user As String, ByVal sessionID As String) As LoginCacheResult
    '    Dim _result As LoginCacheResult = LoginCacheResult.NotMatched
    '    If Not user = String.Empty Then
    '        user = user.ToUpper
    '        Dim _cache As Caching.Cache = HttpContext.Current.Cache
    '        If Not _cache.Item(user) Is Nothing Then
    '            If _cache(user) = "disconnect" Then
    '                _cache.Remove(user)
    '                _result = LoginCacheResult.Disconnect
    '            ElseIf _cache(user) = "full" Then
    '                _result = LoginCacheResult.Matched
    '            Else
    '                If _cache(user).ToString = sessionID Then
    '                    _result = LoginCacheResult.Matched
    '                Else
    '                    _result = LoginCacheResult.NotMatched
    '                End If
    '            End If
    '        Else
    '            CreateLoginCache(user, sessionID)
    '            _result = LoginCacheResult.Created
    '        End If
    '    End If
    '    Return _result
    'End Function

    'Public Sub CreateLoginCache(ByVal user As String, ByVal sessionID As String)
    '    Dim _usuario As New BO.SecUsers
    '    If _usuario.LoadByPrimaryKey(user) Then
    '        HttpContext.Current.Cache.Insert(user, "full", Nothing, Caching.Cache.NoAbsoluteExpiration, Caching.Cache.NoSlidingExpiration, Caching.CacheItemPriority.NotRemovable, Nothing)
    '    Else
    '        HttpContext.Current.Cache.Insert(user, sessionID, Nothing, Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2), Caching.CacheItemPriority.NotRemovable, Nothing)
    '    End If
    'End Sub

    'Public Sub ForceDestroyLoginCache(ByVal user As String)
    '    If Not user = String.Empty Then
    '        user = user.ToUpper
    '        HttpContext.Current.Cache.Remove(user)
    '        HttpContext.Current.Cache(user) = "disconnect"
    '    End If
    'End Sub

    'Public Sub DestroyLoginCache(ByVal user As String)
    '    If Not user = String.Empty Then
    '        user = user.ToUpper
    '        Dim _cache As Caching.Cache = HttpContext.Current.Cache
    '        If Not _cache.Item(user) Is Nothing Then
    '            _cache.Remove(user)
    '        End If
    '    End If
    'End Sub

#End Region

End Module