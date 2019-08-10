Public Class ImagenesUtil
    Private _maxSize As Integer = 120
    Private _currentWidth As Integer = 0
    Private _currentHeight As Integer = 0
    Private _newWidth As Integer = 0
    Private _newHeight As Integer = 0
    Private _ControlValue As Double

    Public Sub New(ByVal maxSize As Integer)
        _maxSize = maxSize
    End Sub

    Public Property MaxSize() As Integer
        Get
            Return _maxSize
        End Get
        Set(ByVal value As Integer)
            _maxSize = value
        End Set
    End Property

    Public Property CurrentWidth() As Integer
        Get
            Return _currentWidth
        End Get
        Set(ByVal value As Integer)
            _currentWidth = value
        End Set
    End Property

    Public Property CurrentHeight() As Integer
        Get
            Return _currentHeight
        End Get
        Set(ByVal value As Integer)
            _currentHeight = value
        End Set
    End Property

    Public ReadOnly Property NewWidth() As Integer
        Get
            Return _newWidth
        End Get
    End Property


    Public ReadOnly Property NewHeight() As Integer
        Get
            Return _newHeight
        End Get
    End Property


    ''' <summary>
    ''' Cambia el alto de la imagen en base a maxsize, mantiene el aspect-ratio 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHeight()
        If (Me._currentHeight > Me._maxSize) Then
            Me._ControlValue = (CType(Me._maxSize, Double) / Me._currentHeight)
            Me._newWidth = CInt(Me._currentWidth * Me._ControlValue)
            Me._newHeight = Me._maxSize
        Else
            Me._newWidth = Me._currentWidth
            Me._newHeight = Me._currentHeight
        End If
    End Sub

    ''' <summary>
    ''' Cambia el ancho de la imagen en base a maxsize, mantiene el aspect-ratio 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWidth()
        If (Me._currentWidth > Me._maxSize) Then

            Me._ControlValue = (CType(Me._maxSize, Double) / Me.CurrentWidth)
            Me._newHeight = CInt(Me._currentHeight * Me._ControlValue)
            Me._newWidth = Me._maxSize

        Else

            Me._newWidth = Me._currentWidth
            Me._newHeight = Me._currentHeight

        End If
    End Sub

    Private Sub SetValues()
        Me._newHeight = 0
        Me._newWidth = 0

        If ((Me._maxSize > 0) _
         And (Me._currentWidth > 0) _
         And (Me._currentHeight > 0)) Then
            If (Me._currentHeight > Me._currentWidth) Then
                SetHeight()
            Else
                SetWidth()
            End If
        End If

    End Sub

    Public Sub Refresh()
        Me.SetValues()
    End Sub

    Public Sub GetSizeFromImages(ByVal argBuffer() As Byte)
        Try
            Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(argBuffer)
            Dim imgInput As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
            Me._currentWidth = imgInput.Width
            Me._currentHeight = imgInput.Height

            Me.SetValues()

        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>
    ''' Obtiene el tamano actual de la imagen en base al buffer, cambia el tamano segun el parametro de ancho o alto especificado
    ''' </summary>
    ''' <param name="argBuffer">buffer con la imagen</param>
    ''' <param name="maxWidth">si es >0 se establece como el ancho maximo</param>
    ''' <param name="maxHeight">si es >0 se establece como el alto maximo</param>
    ''' <remarks></remarks>
    Public Sub GetSizeFromImages(ByVal argBuffer() As Byte, ByVal maxWidth As Integer, ByVal maxHeight As Integer)
        Try
            Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(argBuffer)
            Dim imgInput As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
            Me._currentWidth = imgInput.Width
            Me._currentHeight = imgInput.Height

            If maxWidth > 0 Then
                _maxSize = maxWidth
                SetWidth()
            ElseIf maxHeight > 0 Then
                _maxSize = maxHeight
                SetHeight()
            Else
                Me.SetValues()
            End If

        Catch ex As Exception
        End Try

    End Sub

    Public Sub New()

    End Sub

    Public Sub New(ByVal argWidth As Integer, ByVal argHeight As Integer)
        Me._currentHeight = argHeight
        Me._currentWidth = argWidth
        Me.SetValues()

    End Sub

    Public Shared Function getImagetag(ByVal qPhoto1 As DataSenalesCaribe.Photo) As String

        Dim _ext1 As String = getExtension(qPhoto1.ContentType)
        Dim _class1 As String = getClass(_ext1)
        Dim _link1 As String = String.Empty
        Dim tag1 As String = String.Empty

        _link1 = String.Format("/Admin/vImg/Imagenes.aspx?ID={0}&x=file.{1}", qPhoto1.IDPhoto, _ext1)

        If _ext1 = "swf" OrElse _ext1 = "flv" OrElse _ext1 = "mp4" Then
            ' SWF / FLV / MP4 - Inline
            tag1 = String.Format("<div rel='{0};' class='{1}'></div>", _link1, _class1)
        Else

            tag1 = String.Format("<img src='{0}' class='{1}'/>", _link1, _class1)
        End If
        Return tag1
    End Function

    'Public Shared Function getImagetag(ByVal IDPhoto As Integer, ByVal ContentType As String, ByVal Height As Integer, ByVal Width As Integer, ByVal Description As String) As String
    '    Return getImagetag(IDPhoto, ContentType, Description, False)
    'End Function

    Public Shared Function getImagetag(ByVal IDPhoto As Integer, ByVal ContentType As String, ByVal Description As String, ByVal bgImage As Boolean) As String
        Dim _ext1 As String = getExtension(ContentType)
        Dim _class1 As String = getClass(_ext1)
        Dim _link1 As String = String.Empty
        Dim _link2 As String = String.Empty
        Dim tag1 As String = String.Empty

        _link1 = String.Format("/Admin/vImg/Imagenes.aspx?ID={0}&x=file.{1}", IDPhoto, _ext1)
        If _ext1 = "flv" Then
            _link2 = String.Format("/Admin/vImg/Imagenes.aspx%3FID%3D{0}%26x%3Dfile.{1}", IDPhoto, _ext1)
        Else
            _link2 = _link1
        End If

        If _ext1 = "swf" OrElse _ext1 = "flv" OrElse _ext1 = "mp4" Then
            ' SWF / FLV / MP4 - Inline
            tag1 = String.Format("<a href='{4}' rel='shadowbox[gallery];player={3};'><div rel='{0};' class='{1}'></div></a>", _link1, _class1, _ext1, _link2)
        Else
            If bgImage Then
                tag1 = String.Format("<a href='{0}' rel='shadowbox[gallery];player=img;' title='{3}'><i class='{1}' style='background-image: url({0}>&x=file.jpg&thum=1);'></i></a>", _link1, _class1, IDPhoto.ToString, Description)
            Else
                tag1 = String.Format("<a href='{0}' rel='shadowbox[gallery];player=img;'><img src='{0}&thum=1' class='{1}' alt='{3}'/></a>", _link1, _class1, IDPhoto.ToString, Description)
            End If

        End If
        Return tag1
    End Function

    Public Shared Function getExtension(ByVal contentType As String) As String
        Dim _ext As String = String.Empty
        If contentType IsNot Nothing Then
            If contentType = "application/x-shockwave-flash" Then
                _ext = "swf"
            ElseIf contentType.StartsWith("video/") Then
                _ext = "flv"
            ElseIf contentType = "image/jpeg" Then
                _ext = "jpg"
            ElseIf contentType = "image/png" Then
                _ext = "png"
            ElseIf contentType = "image/gif" Then
                _ext = "gif"
            End If
        End If
        Return _ext
    End Function

    Public Shared Function getClass(ByVal _ext As String) As String
        Dim _class As String = String.Empty
        If _ext = "swf" Then
            _class = "adswf"
        ElseIf _ext = "flv" Then
            _class = "adflv"
        ElseIf _ext = "jpeg" OrElse _ext = "png" OrElse _ext = "gif" Then
            _class = "adimg"
        Else
            _class = "adimg"
        End If
        Return _class
    End Function

    Public Shared Function getContentType(ByVal filename As String, ByVal contentType As String) As String
        Dim _contentType As String = String.Empty
        Dim _ext As String = filename.Split(".")(1)
        Select Case _ext
            Case "swf"
                _contentType = "application/x-shockwave-flash"
            Case "flv"
                _contentType = "video/x-flv"
            Case "mp4"
                _contentType = "video/mp4"
            Case "jpg", "jpeg"
                _contentType = "image/jpeg"
            Case "png"
                _contentType = "image/png"
            Case "gif"
                _contentType = "image/gif"
            Case Else
                _contentType = contentType
        End Select

        Return _contentType
    End Function

End Class

Public Class UtilClass

    Public Shared Function byteArrayToImage(ByVal byteArrayIn() As Byte) As System.Drawing.Image
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(byteArrayIn)
        Return System.Drawing.Image.FromStream(ms)

    End Function

    Public Shared Function byteArrayToImage(ByVal byteArrayIn() As Byte, ByVal Ubicacion As String) As System.Drawing.Image
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(byteArrayIn)
        Dim returnImage As System.Drawing.Image = System.Drawing.Image.FromStream(ms)
        returnImage.Save(Ubicacion)
        Return returnImage

    End Function

    Public Shared Function byteArrayToThumbNail(ByVal byteArrayIn() As Byte, ByVal Width As Integer, ByVal Height As Integer) As System.Drawing.Image
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(byteArrayIn)
        'Return System.Drawing.Image.FromStream(ms, True).GetThumbnailImage(Width, Height, New System.Drawing.Image.GetThumbnailImageAbort(UtilClass.ImageAbortCallback), IntPtr.Zero)
        Return System.Drawing.Image.FromStream(ms, True).GetThumbnailImage(Width, Height, New System.Drawing.Image.GetThumbnailImageAbort(AddressOf UtilClass.ImageAbortCallback), IntPtr.Zero)

    End Function

    Private Shared Function ImageAbortCallback() As Boolean
        Return True
    End Function

    Public Shared Function imageFromDiskToByteArray(ByVal FromUbicacion As String) As Byte()
        Return imageToByteArray(System.Drawing.Image.FromFile(FromUbicacion))
    End Function

    Public Shared Function imageToByteArray(ByVal imageIn As System.Drawing.Image) As Byte()
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream()
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Return ms.ToArray()

    End Function


End Class