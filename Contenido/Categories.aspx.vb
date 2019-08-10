Imports DataSenalesCaribe

Partial Public Class Categories
    Inherits System.Web.UI.Page

    Private Property IDPhotoDefault() As Integer
        Get
            If (ViewState("IDPhotoDefault") Is Nothing) Then ViewState("IDPhotoDefault") = -1
            Return CType(ViewState("IDPhotoDefault"), Integer)
        End Get
        Set(ByVal value As Integer)
            ViewState("IDPhotoDefault") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            If IsNothing(Session("UserInfo")) Then
                Response.Redirect("~/Account/Login.aspx", False)
                Return
            End If

            Me.getCategoriesType()
            Me.getParentCategory()
            Me.CategoryDoSearch()
        End If
    End Sub

    Private Sub getCategoriesType()
        Try
            Dim db As New DataSenalesCaribe.SecurityDataContext(Comun.GetConnString)

            Dim categoriesType = (From i In db.Parametros _
                                  Where i.IDGroup = Comun.Enums.Parametros.tipos_categorias _
                                  Select i.CustomInt1, i.Descripcion)

            Me.ddlEditTipoCategory.DataSource = categoriesType
            Me.ddlEditTipoCategory.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub getParentCategory()
        Try
            Dim db As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim parentsCat = (From i In db.Categories Where i.IsDeleted = False Select i.IDCategory, i.CatName Order By CatName)

            Me.ddlParent.DataSource = parentsCat
            Me.ddlParent.DataBind()

            ddlParent.Items.Insert(0, New ListItem("-- Seleccione --", "-1"))
            ddlParent.SelectedValue = "-1"
        Catch ex As Exception

        End Try
    End Sub


    Private Sub CategoryDoSearch()
        Try
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryMenu = From p In portalDC.Categories _
                            Where p.IsDeleted = False _
                            Select p _
                            Order By p.OrderBy Ascending

            Me.tvCat.Nodes.Clear()
            For Each item In queryMenu
                If (item.IDParent = "-1") Then
                    Me.tvCat.Nodes.Add(Me.GetNodeItem(item.IDCategory, item.CatName, queryMenu))
                End If
            Next

        Catch ex As Exception
            'Me.lblMsg.Text += String.Format("Error buscando categorias: {0}.<br />", ex.Message)
            'Comun.SetError(String.Format("Error buscando categorias: {0}", ex.Message))
            'Comun.SaveLog(ex.ToString())
        End Try

    End Sub

    Private Function GetNodeItem(ByVal idCat As Integer _
                                 , ByVal sName As String _
                                 , ByVal queryMenu As System.Linq.IQueryable(Of DataSenalesCaribe.Category)) As TreeNode
        Dim tnitem As New TreeNode

        Try
            tnitem.Value = idCat
            tnitem.Text = sName

            Dim qMenu = From p In queryMenu _
                        Where p.IDParent = idCat _
                        Select p Order By p.OrderBy Ascending

            If (qMenu.Count > 0) Then
                For Each item In qMenu
                    tnitem.ChildNodes.Add(Me.GetNodeItem(item.IDCategory, item.CatName, queryMenu))
                Next
            End If

        Catch ex As Exception
            'Comun.SaveLog(ex.ToString())
        End Try
        Return tnitem
    End Function

    Private Sub CategoryClearForm()

        Me.lblID.Text = "-1"
        Me.lblEditCatCategory.Text = "[RAIZ]"
        Me.ddlParent.SelectedValue = -1
        Me.txtEditCatName.Text = String.Empty
        Me.txtEditCatDescripcion.Text = String.Empty
        Me.txtEditCatOrderBy.Text = String.Empty
        Me.txtEditCatURL.Text = String.Empty
        Me.ddlEditTipoCategory.SelectedIndex = 0
        Me.cbEditIsMenu.Checked = False
        Me.imgDefault.ImageUrl = "~/vImg/Imagenes.aspx?ID={0}"
    End Sub

    Private Function CategoryEdit(ByVal idCat As Integer) As Boolean
        Try
            Me.CategoryClearForm()
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryCategory = (From p In portalDC.Categories _
                            Where p.IDCategory = idCat _
                                And p.IsDeleted = False _
                            Select p _
                            Order By p.OrderBy Ascending).FirstOrDefault()


            If (Not queryCategory Is Nothing) Then

                Me.lblID.Text = queryCategory.IDCategory
                If (Not queryCategory.IDParent Is Nothing) Then ddlParent.SelectedValue = queryCategory.IDParent
                Me.lblEditCatCategory.Text = Me.GetCategoryPath(Me.ddlParent.SelectedValue)
                If (Not queryCategory.pCategoryType = Nothing) Then Me.ddlEditTipoCategory.SelectedValue = queryCategory.pCategoryType
                If (Not queryCategory.CatName Is Nothing) Then Me.txtEditCatName.Text = queryCategory.CatName
                If (Not queryCategory.CatDescription Is Nothing) Then Me.txtEditCatDescripcion.Text = queryCategory.CatDescription
                If (Not queryCategory.CatURL Is Nothing) Then Me.txtEditCatURL.Text = queryCategory.CatURL
                If (Not queryCategory.IsMenu Is Nothing) Then Me.cbEditIsMenu.Checked = queryCategory.IsMenu
                If (Not queryCategory.OrderBy Is Nothing) Then Me.txtEditCatOrderBy.Text = queryCategory.OrderBy

                If Not queryCategory.IDPhoto Is Nothing Then
                    Me.IDPhotoDefault = queryCategory.IDPhoto
                    Me.imgDefault.ImageUrl = String.Format("~/vImg/Imagenes.aspx?ID={0}", queryCategory.IDPhoto)
                    Me.imgDefault.Visible = True
                Else
                    Me.IDPhotoDefault = -1
                    Me.imgDefault.Visible = False
                End If
            Else
                Throw New Exception("Registro no puede ser encontrado.")
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function GetCategoryPath(ByVal idCat As Integer) As String
        Dim sPath As String = String.Empty
        Try
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim queryCategory = From p In portalDC.Categories _
                            Where p.IDCategory = idCat _
                            Select p _
                            Order By p.OrderBy Ascending

            If (queryCategory.Count > 0) Then
                For Each item In queryCategory
                    sPath = String.Format("{0} :: [{1}] " _
                                              , Me.GetCategoryPath(item.IDParent, queryCategory) _
                                              , item.CatName)

                Next
            Else
                sPath = "[RAIZ]"
            End If
        Catch ex As Exception
        End Try
        Return sPath
    End Function

    Private Function GetCategoryPath(ByVal idCat As Integer _
                                     , ByVal queryCategory As System.Linq.IQueryable(Of DataSenalesCaribe.Category)) As String
        Dim sPath As String = String.Empty
        Try
            Dim qCategory = From p In queryCategory _
                        Where p.IDCategory = idCat _
                        Select p Order By p.OrderBy Ascending

            If (qCategory.Count > 0) Then


                For Each item In qCategory
                    sPath = String.Format("{0} :: [{1}] " _
                                          , Me.GetCategoryPath(item.IDParent, queryCategory) _
                                          , item.CatName)

                Next
            Else
                sPath = "[RAIZ]"
            End If


        Catch ex As Exception
        End Try

        Return sPath
    End Function

    Private Function CategoryIsValid() As Boolean
        Dim b As Boolean = True

        If (Not IsNumeric(Me.lblID.Text) Or String.IsNullOrEmpty(Me.lblID.Text)) Then
            b = False
        End If

        If (String.IsNullOrEmpty(Me.txtEditCatName.Text)) Then
            Me.txtEditCatName.Focus()
            b = False
        End If

        If (Not String.IsNullOrEmpty(Me.txtEditCatOrderBy.Text) And Not IsNumeric(Me.txtEditCatOrderBy.Text)) Then
            Me.txtEditCatOrderBy.Focus()
            b = False
        End If

        If (Me.ddlEditTipoCategory.SelectedValue = Comun.Enums.CategoryType.link) And String.IsNullOrEmpty(Me.txtEditCatURL.Text) Then
            Me.txtEditCatURL.Focus()
            b = False
        End If

        Return b
    End Function

    Private Function CategorySave() As Boolean
        Try
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oCat As DataSenalesCaribe.Category

            If (Me.lblID.Text = "-1") Then
                oCat = New DataSenalesCaribe.Category

                oCat.Create_User = HttpContext.Current.User.Identity.Name.ToString()
                oCat.Create_Date = Now
            Else
                oCat = (From p In portalDC.Categories _
                        Where p.IDCategory = CInt(Me.lblID.Text) _
                        Select p).First()
            End If

            oCat.IDParent = Me.ddlParent.SelectedValue
            oCat.CatName = Me.txtEditCatName.Text
            oCat.CatDescription = Me.txtEditCatDescripcion.Text
            oCat.CatURL = Me.txtEditCatURL.Text
            oCat.pCategoryType = Me.ddlEditTipoCategory.SelectedValue

            If (Not String.IsNullOrEmpty(Me.txtEditCatOrderBy.Text)) Then oCat.OrderBy = CInt(Me.txtEditCatOrderBy.Text) Else oCat.OrderBy = 100

            oCat.IsMenu = Me.cbEditIsMenu.Checked

            oCat.Update_User = HttpContext.Current.User.Identity.Name.ToString()
            oCat.Update_Date = Now

            oCat.IsDeleted = False

            'to do
            Dim _idphoto As Integer = SavePhoto(portalDC)
            If _idphoto <> -1 Then
                If oCat.IDPhoto IsNot Nothing Then
                    Dim _idphotoold As Integer = oCat.IDPhoto
                    DeleteImage(portalDC, _idphotoold)
                End If
                oCat.IDPhoto = _idphoto
            End If

            If (Me.lblID.Text = "-1") Then portalDC.Categories.InsertOnSubmit(oCat)

            portalDC.SubmitChanges()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function CategoryDelete(ByVal argIDCat As Integer) As Boolean
        Try

            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oCat As DataSenalesCaribe.Category

            oCat = (From p In portalDC.Categories _
                    Where p.IDCategory = argIDCat _
                    Select p).FirstOrDefault()

            If (oCat Is Nothing) Then Throw New Exception("Categoria especificada no pudo ser encontrada.")

            oCat.IsDeleted = False
            oCat.Update_Date = DateTime.Now
            oCat.Update_User = HttpContext.Current.User.Identity.Name.ToString()

            portalDC.SubmitChanges()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub tvCat_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tvCat.SelectedNodeChanged
        Me.CategoryEdit(CInt(Me.tvCat.SelectedNode.Value))
    End Sub

    Protected Sub lnkGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkGuardar.Click
        If (Me.CategoryIsValid()) Then
            If (Me.CategorySave()) Then
                ' codigo despues de grabar aqui
                Me.CategoryClearForm()
                Me.CategoryDoSearch()
            End If
        End If
    End Sub

    Protected Sub lnkCmdGoBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCmdGoBack.Click
        If (Not Me.tvCat.SelectedNode Is Nothing) Then
            Me.tvCat.SelectedNode.Selected = False
        End If
        Me.CategoryClearForm()
    End Sub

    Protected Sub lnkCmdAddChild_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCmdAddChild.Click
        If (Me.lblID.Text <> "-1") Then
            Dim iCat As Integer = CInt(Me.lblID.Text)
            Me.CategoryClearForm()
            Me.ddlParent.SelectedValue = iCat
            Me.lblEditCatCategory.Text = Me.GetCategoryPath(iCat)
        End If
    End Sub

    Protected Sub lnkCmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCmdDelete.Click
        If (IsNumeric(Me.lblID.Text)) Then
            If (Me.CategoryDelete(CInt(Me.lblID.Text))) Then
                Me.CategoryClearForm()
                Me.CategoryDoSearch()
            End If
        End If
    End Sub

    Private Function SavePhoto(db As DataSenalesCaribe.ContentDataContext) As Integer

        Dim _idPhoto As Integer = -1

        If (Me.FileUpload1.HasFile) Then
            Dim oImg As ImagenesUtil = New ImagenesUtil()

            Dim oPhoto As New DataSenalesCaribe.Photo

            Dim _file As HttpPostedFile = Me.FileUpload1.PostedFile
            Dim fileSize As Integer

            fileSize = Convert.ToInt32(_file.InputStream.Length) + 1
            Dim _buffer(fileSize) As Byte

            _file.InputStream.Read(_buffer, 0, fileSize)

            oImg.GetSizeFromImages(_buffer)

            oPhoto.ContentType = _file.ContentType
            oPhoto.ContentLength = _file.ContentLength

            oPhoto.PhotoSmallName = String.Format("~/vImg/Imagenes.aspx?ID={0}", oPhoto.IDPhoto) ' System.IO.Path.GetFileName(_file.FileName)
            oPhoto.PhotoSmall = UtilClass.imageToByteArray(UtilClass.byteArrayToThumbNail(_buffer, oImg.NewWidth, oImg.NewHeight))

            oPhoto.PhotoLargeName = System.IO.Path.GetFileName(_file.FileName)
            oPhoto.PhotoLarge = _buffer

            oPhoto.Update_User = HttpContext.Current.User.Identity.Name.ToString()
            oPhoto.Update_Date = DateTime.Now

            oPhoto.IsDeleted = False
            db.Photos.InsertOnSubmit(oPhoto)

            db.SubmitChanges()

            oPhoto.PhotoSmallName = String.Format("~/vImg/Imagenes.aspx?ID={0}&thum=true", oPhoto.IDPhoto)
            oPhoto.PhotoLargeName = String.Format("~/vImg/Imagenes.aspx?ID={0}", oPhoto.IDPhoto)

            db.SubmitChanges()
            _idPhoto = oPhoto.IDPhoto
        End If

        Return _idPhoto
    End Function

    Private Sub DeleteImage(db As DataSenalesCaribe.ContentDataContext, idPhoto As Integer)
        Dim _photoOld = db.Photos.FirstOrDefault(Function(d) d.IDPhoto = idPhoto)
        If _photoOld IsNot Nothing Then
            db.Photos.DeleteOnSubmit(_photoOld)
        End If
    End Sub

    Private Sub cmdDeleteImage_Click(sender As Object, e As System.EventArgs) Handles cmdDeleteImage.Click
        If Me.IDPhotoDefault <> -1 Then
            Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)

            Dim oCat = portalDC.Categories.FirstOrDefault(Function(d) d.IDCategory = CInt(Me.lblID.Text))
            oCat.IDPhoto = Nothing

            DeleteImage(portalDC, Me.IDPhotoDefault)

            portalDC.SubmitChanges()

            Me.IDPhotoDefault = -1
            Me.imgDefault.Visible = False
        End If
    End Sub
End Class