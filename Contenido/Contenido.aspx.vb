Imports DataSenalesCaribe

Partial Public Class Contenido
    Inherits System.Web.UI.Page

    Private Property IDPhoto() As Integer
        Get
            If (ViewState("IDPhoto") Is Nothing) Then ViewState("IDPhoto") = -1
            Return CType(ViewState("IDPhoto"), Integer)
        End Get
        Set(ByVal value As Integer)
            ViewState("IDPhoto") = value
        End Set
    End Property

    Private Property IDContent() As Integer
        Get
            If (ViewState("IDContent") Is Nothing) Then ViewState("IDContent") = -1
            Return CType(ViewState("IDContent"), Integer)
        End Get
        Set(ByVal value As Integer)
            ViewState("IDContent") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            If IsNothing(Session("UserInfo")) Then
                Response.Redirect("~/Account/Login.aspx", False)
                Return
            End If

            'Me.cmdAddNew.Visible = VerificaAcceso("Mantenimiento.Contenido.Agregar")
            Me.FillCat()
            Me.ShowSearchForm()
            Me.DoSearch()
        End If
    End Sub

    Private Sub ShowNoAccess()
        Me.MultiView1.SetActiveView(Me.View1)
    End Sub

    Private Sub ShowSearchForm()
        Me.MultiView1.SetActiveView(Me.View2)
    End Sub

    Private Sub ShowAddEditForm()
        Me.MultiView1.SetActiveView(Me.View3)
    End Sub

    Private Sub DoSearch()
        Try

            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)
            Dim cat As Integer = lstCategoria.SelectedValue

            Dim q = portaldc.Content_Categories.Where(Function(d) d.IDCategory = cat).Select(Function(d) d.IDContent)

            Dim queryContent = From p In portaldc.Contents _
                               Where (String.IsNullOrEmpty(Me.txtSearchBox.Text) _
                                        Or p.Title.Contains(Me.txtSearchBox.Text)) _
                                        And (cat = -1 OrElse q.Contains(p.IDContent)) _
                               Select p

            Me.gvContenido.DataSource = queryContent
            Me.gvContenido.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ContentClearForm()
        Me.lblID.Text = "-1"
        Me.IDPhoto = -1

        Me.freeEditorContenido.Text = String.Empty

        Me.txtEditTitulo.Text = String.Empty
        Me.txtEditOrderBy.Text = String.Empty

        Me.cbIsPublished.Checked = False

        Me.PopCalPublishDate.DateValue = DateTime.Now
        Me.IDContent = -1
        Me.ListContentCategory()
        Me.CheckCategoryByContent(-1)
    End Sub

    Private Function ContentIsFormValid() As Boolean
        Dim b As Boolean = True

        If (String.IsNullOrEmpty(Me.PopCalPublishDate.SelectedDate)) Then
            Me.txtEditPublishDate.Focus()
            b = False
        End If

        If (String.IsNullOrEmpty(Me.txtEditTitulo.Text)) Then
            Me.txtEditTitulo.Focus()
            b = False
        End If

        If (Not String.IsNullOrEmpty(Me.txtEditOrderBy.Text)) AndAlso (Not IsNumeric(Me.txtEditOrderBy.Text)) Then
            Me.txtEditOrderBy.Focus()
            b = False
        End If

        Return b
    End Function

    Private Sub ListContentCategory()
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)
            Dim queryCat = From p In portaldc.Categories Where p.IsDeleted = False Select p

            Me.cblEditCategories.Items.Clear()

            For Each item As DataSenalesCaribe.Category In queryCat
                Me.cblEditCategories.Items.Add(New ListItem(item.CatName, item.IDCategory))
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckCategoryByContent(ByVal idCont As Integer)
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryCat = From p In portaldc.Content_Categories _
              Where p.IDContent = idCont And p.IsDeleted = False _
              Select p

            For Each item In queryCat
                If (Not IsNothing(Me.cblEditCategories.Items.FindByValue(item.IDCategory))) Then
                    Me.cblEditCategories.Items.FindByValue(item.IDCategory).Selected = True
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Function ContentSave() As Boolean
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oContent As DataSenalesCaribe.Content

            If (Me.lblID.Text = "-1") Then
                oContent = New DataSenalesCaribe.Content

                oContent.Create_User = HttpContext.Current.User.Identity.Name.ToString()
                oContent.Create_Date = Now
            Else
                oContent = (From p In portaldc.Contents _
                  Where p.IDContent = CInt(Me.lblID.Text) _
                  Select p).FirstOrDefault()
            End If

            If (Not String.IsNullOrEmpty(Me.PopCalPublishDate.SelectedDate)) Then oContent.PublishDate = Me.PopCalPublishDate.DateValue
            If (Not String.IsNullOrEmpty(Me.PopCalExpiredDate.SelectedDate)) Then oContent.Expired_Date = Me.PopCalExpiredDate.DateValue

            oContent.HTMLContent = Me.freeEditorContenido.Text

            If Me.txtEditTitulo.Text.Length > 500 Then Me.txtEditTitulo.Text = Me.txtEditTitulo.Text.Substring(0, 500)
            oContent.Title = Me.txtEditTitulo.Text

            oContent.IsPublished = Me.cbIsPublished.Checked
            oContent.OrderBy = CInt(Me.txtEditOrderBy.Text)

            oContent.Update_User = HttpContext.Current.User.Identity.Name.ToString()
            oContent.Update_Date = Now

            oContent.IsDeleted = False

            For Each item As ListItem In Me.cblEditCategories.Items
                Dim _listItem As ListItem = item
                If (item.Selected) Then
                    Dim queryCat = oContent.Content_Categories.Where(Function(p) p.IDCategory = CInt(_listItem.Value.ToString()))

                    If (queryCat.Count > 0) Then
                        Dim oCat = queryCat.First()
                        oCat.IsDeleted = False
                        oCat.Update_Date = DateTime.Now
                        oCat.Update_User = HttpContext.Current.User.Identity.Name.ToString()
                    Else

                        Dim oCat As New DataSenalesCaribe.Content_Category
                        oCat.IDCategory = CInt(item.Value.ToString())
                        oCat.Create_Date = DateTime.Now
                        oCat.Create_User = HttpContext.Current.User.Identity.Name.ToString()

                        oCat.Update_User = HttpContext.Current.User.Identity.Name.ToString()
                        oCat.Update_Date = DateTime.Now

                        oCat.IsDeleted = False

                        oContent.Content_Categories.Add(oCat)

                    End If
                Else
                    Dim queryCat = oContent.Content_Categories.Where(Function(p) p.IDCategory = CInt(_listItem.Value.ToString()))
                    If (queryCat.Count > 0) Then
                        queryCat.First().IsDeleted = True
                        queryCat.First().Update_Date = DateTime.Now
                        queryCat.First().Update_User = HttpContext.Current.User.Identity.Name.ToString()
                    End If
                End If
            Next

            If (Me.lblID.Text = "-1") Then portaldc.Contents.InsertOnSubmit(oContent)

            portaldc.SubmitChanges()
            Me.lblID.Text = oContent.IDContent
            Me.IDContent = oContent.IDContent

            portaldc.Dispose()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ContentEdit(ByVal idCont As Integer) As Boolean
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)
            Dim oContent = (From p In portaldc.Contents _
               Where p.IDContent = idCont _
               Select p).First()


            Me.lblID.Text = oContent.IDContent
            Me.PopCalPublishDate.DateValue = oContent.PublishDate
            If oContent.Expired_Date.HasValue Then Me.PopCalExpiredDate.DateValue = oContent.Expired_Date
            If (Not oContent.Title Is DBNull.Value) Then Me.txtEditTitulo.Text = oContent.Title

            If (Not IsNothing(oContent.OrderBy)) Then Me.txtEditOrderBy.Text = oContent.OrderBy.ToString()

            Me.freeEditorContenido.Text = oContent.HTMLContent

            If (Not oContent.IsPublished Is Nothing) Then Me.cbIsPublished.Checked = oContent.IsPublished

            Me.CheckCategoryByContent(oContent.IDContent)

            Me.IDContent = oContent.IDContent
            'If (Not oContent.IDPhotoDefault Is Nothing) Then Me.IDPhotoDefault = oContent.IDPhotoDefault

            PhotoDoSearch()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function PhotoSave() As Boolean
        Try

            Dim oImg As ImagenesUtil = New ImagenesUtil()

            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)
            Dim oPhoto As DataSenalesCaribe.Photo

            If (Me.IDPhoto > 0) Then
                oPhoto = portalDC.Photos.First(Function(o) o.IDPhoto = Me.IDPhoto)
            Else
                If (Not Me.FileUpload1.HasFile) Then Throw New Exception("No se ha especificado ningun archivo.")
                oPhoto = New DataSenalesCaribe.Photo
                oPhoto.Create_User = HttpContext.Current.User.Identity.Name.ToString()
                oPhoto.Create_Date = DateTime.Now

                Dim oPhotoByContent As New DataSenalesCaribe.ContentPhoto
                oPhotoByContent.IDContent = Me.IDContent

                oPhotoByContent.Create_Date = DateTime.Now
                oPhotoByContent.Create_User = HttpContext.Current.User.Identity.Name.ToString()
                oPhotoByContent.IsDeleted = False
                oPhotoByContent.Update_User = HttpContext.Current.User.Identity.Name.ToString()
                oPhotoByContent.Update_Date = DateTime.Now

                oPhoto.ContentPhotos.Add(oPhotoByContent)
            End If

            If (Me.FileUpload1.HasFile) Then

                Dim _file As HttpPostedFile = Me.FileUpload1.PostedFile
                Dim fileSize As Integer

                fileSize = Convert.ToInt32(_file.InputStream.Length) + 1
                Dim _buffer(fileSize) As Byte

                _file.InputStream.Read(_buffer, 0, fileSize)

                oPhoto.ContentType = ImagenesUtil.getContentType(_file.FileName, _file.ContentType)
                oPhoto.ContentLength = _file.ContentLength

                'oPhoto.PhotoSmallName = String.Format("~/vImg/Imagenes.aspx?ID={0}", oPhoto.IDPhoto) ' System.IO.Path.GetFileName(_file.FileName)
                Dim _ext1 As String = ImagenesUtil.getExtension(_file.ContentType)
                If oPhoto.ContentType.StartsWith("image/") Then
                    oImg.GetSizeFromImages(_buffer)
                    oPhoto.PhotoSmall = UtilClass.imageToByteArray(UtilClass.byteArrayToThumbNail(_buffer, oImg.NewWidth, oImg.NewHeight))
                Else
                    oPhoto.PhotoSmall = _buffer
                End If

                'oPhoto.PhotoLargeName = System.IO.Path.GetFileName(_file.FileName)
                oPhoto.PhotoLarge = _buffer

            End If

            oPhoto.Description = Me.txtEditPhotoComent.Text

            oPhoto.Update_User = HttpContext.Current.User.Identity.Name.ToString()
            oPhoto.Update_Date = DateTime.Now

            oPhoto.IsDeleted = False
            If (Not Me.IDPhoto > 0) Then portalDC.Photos.InsertOnSubmit(oPhoto)

            portalDC.SubmitChanges()

            oPhoto.PhotoSmallName = String.Format("~/vImg/Imagenes.aspx?ID={0}&thum=true", oPhoto.IDPhoto)
            oPhoto.PhotoLargeName = String.Format("~/vImg/Imagenes.aspx?ID={0}", oPhoto.IDPhoto)

            portalDC.SubmitChanges()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Indica si los campos requeridos para registrar una foto son correctos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PhotoIsFormValid() As Boolean
        Dim b As Boolean = True
        If (Not Me.IDPhoto < 0) Then
            If (Not Me.FileUpload1.HasFile) Then
                b = False
                Me.FileUpload1.Focus()
            End If
        End If

        If (Not Me.IDContent > 0) Then
            b = False
        End If

        Return b
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iPhoto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PhotoDelete(ByVal iPhoto As Integer) As Boolean
        Try

            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oPhotoByContenido = portalDC.ContentPhotos.FirstOrDefault(Function(o) o.IDPhoto = iPhoto)
            If oPhotoByContenido IsNot Nothing Then
                oPhotoByContenido.IsDeleted = True
            End If

            Dim oPhoto = portalDC.Photos.FirstOrDefault(Function(o) o.IDPhoto = iPhoto)
            If oPhoto IsNot Nothing Then
                oPhoto.IsDeleted = True
            End If

            portalDC.SubmitChanges()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub PhotoDoSearch()
        Try
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryPhoto = From p In portalDC.ContentPhotos _
              Where p.IsDeleted = False And p.IDContent = Me.IDContent _
              Select p.Photo

            Me.lvImages.DataSource = queryPhoto
            Me.lvImages.DataBind()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub PhotoClearForm()
        Me.IDPhoto = -1
        Me.txtEditPhotoComent.Text = String.Empty
    End Sub

    Private Function PhotoSetDefault(ByVal argIDPhoto As Integer) As Boolean
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oContent = portaldc.Contents.First(Function(o) o.IDContent = Me.IDContent)

            'oContent.IDPhotoDefault = argIDPhoto
            oContent.Update_Date = DateTime.Now
            oContent.Update_User = HttpContext.Current.User.Identity.Name.ToString()

            portaldc.SubmitChanges()

            'If (Not oContent.IDPhotoDefault Is Nothing) Then Me.IDPhotoDefault = oContent.IDPhotoDefault Else Me.IDPhotoDefault = -1


            Return True

        Catch ex As Exception

            Return False
        End Try
    End Function

    Private Function PhotoSetNoDefault(ByVal argIDPhoto As Integer) As Boolean
        Try
            'Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            'Dim oContent = portaldc.Contents.First(Function(o) o.IDContent = Me.IDContent And o.IDPhotoDefault = argIDPhoto)

            'oContent.IDPhotoDefault = Nothing
            'oContent.Update_Date = DateTime.Now
            'oContent.Update_User = HttpContext.Current.User.Identity.Name.ToString()

            'portaldc.SubmitChanges()


            'If (Not oContent.IDPhotoDefault Is Nothing) Then Me.IDPhotoDefault = oContent.IDPhotoDefault Else Me.IDPhotoDefault = -1
            Return True

        Catch ex As Exception

            Return False
        End Try
    End Function

    Private Function PhotoDeletePhoto(ByVal argIDPhoto As Integer) As Boolean
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim contPhoto = portaldc.ContentPhotos.FirstOrDefault(Function(o) o.IDContent = Me.IDContent And o.IDPhoto = argIDPhoto)
            If (Not contPhoto Is Nothing) Then
                contPhoto.IsDeleted = True

                contPhoto.Photo.IsDeleted = True

                portaldc.SubmitChanges()
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function PhotoEdit(ByVal argIDPhoto As Integer) As Boolean
        Try
            Dim portaldc As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oPhoto = portaldc.Photos.FirstOrDefault(Function(o) o.IDPhoto = argIDPhoto)
            If (oPhoto Is Nothing) Then
                Throw New Exception("Registro no econtrado.")
            End If

            Me.IDPhoto = oPhoto.IDPhoto
            Me.txtEditPhotoComent.Text = oPhoto.Description
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Me.DoSearch()
    End Sub

    Private Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Me.ShowAddEditForm()
        Me.ContentClearForm()
    End Sub

    Protected Sub CmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        'If Not VerificaAcceso("Mantenimiento.Contenido.Agregar") Then
        '    Response.Redirect("~/Seguridad/Acceso_Denegado.aspx", False)
        '    Return
        'End If
        If (Me.ContentIsFormValid()) Then
            If (Me.ContentSave()) Then
                Me.ShowSearchForm()
                Me.DoSearch()
            End If
        End If

    End Sub

    Protected Sub cmdGoBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGoBack.Click
        Me.ShowSearchForm()
        Me.DoSearch()
    End Sub

    Protected Sub gvContenido_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvContenido.RowCommand
        Dim _idContent As Integer = CInt(e.CommandArgument.ToString())
        Select Case e.CommandName
            Case "Editar"
                If Not (CType(Session("UserInfo"), SeguridadClass.LoginClass).UserType = SeguridadClass.LoginClass.eUserLevel.Todos) Then
                    Response.Redirect("~/Default.aspx?msg=Acceso Denegado")
                    Return
                Else
                    Me.ContentClearForm()
                    If (ContentEdit(_idContent)) Then
                        Me.ShowAddEditForm()
                    End If
                End If
            Case "Borrar"
                If Not (CType(Session("UserInfo"), SeguridadClass.LoginClass).UserType = SeguridadClass.LoginClass.eUserLevel.Todos) Then
                    Response.Redirect("~/Default.aspx?msg=Acceso Denegado")
                    Return
                Else
                    Dim db As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString())
                    Dim record = db.Contents.FirstOrDefault(Function(d) d.IDContent = _idContent)
                    If record IsNot Nothing Then
                        db.ContentPhotos.DeleteAllOnSubmit(record.ContentPhotos)
                        db.Content_Categories.DeleteAllOnSubmit(record.Content_Categories)
                        db.Contents.DeleteOnSubmit(record)
                        db.SubmitChanges()
                    End If
                End If
        End Select
    End Sub

    Protected Sub cmdUploadImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUploadImage.Click
        If (Me.PhotoIsFormValid()) Then
            If (Me.PhotoSave()) Then
                Me.PhotoDoSearch()
                Me.PhotoClearForm()
            End If
        End If
    End Sub

    Private Sub lvImages_ItemCommand(sender As Object, e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lvImages.ItemCommand
        Select Case e.CommandName
            Case "Editar"
                Me.PhotoClearForm()
                Me.PhotoEdit(CInt(e.CommandArgument.ToString()))
            Case "Borrar"
                If (IsNumeric(e.CommandArgument.ToString())) Then
                    If (Me.PhotoDeletePhoto(CInt(e.CommandArgument.ToString()))) Then
                        Me.PhotoDoSearch()
                    End If
                End If
        End Select
    End Sub

    Private Sub cmdLimpiar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLimpiar.Click
        Me.lstCategoria.SelectedValue = -1
        Me.txtSearchBox.Text = String.Empty
        Me.DoSearch()
    End Sub

    Private Sub FillCat()
        Try
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryCategory = From p In portalDC.Categories Where p.IsDeleted = False Select p Order By p.CatName Ascending

            Me.lstCategoria.DataSource = queryCategory
            Me.lstCategoria.DataBind()

            Me.lstCategoria.Items.Insert(0, New ListItem("-- Seleccione --", "-1"))
            Me.lstCategoria.SelectedValue = "-1"
        Catch ex As Exception

        End Try
    End Sub


End Class