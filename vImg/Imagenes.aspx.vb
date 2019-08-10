Imports DataSenalesCaribe

Partial Public Class Imagenes
    Inherits System.Web.UI.Page

    Private Sub DoImageRender()
        Try

            If (Not Request.QueryString("id") Is Nothing) Then
                If (IsNumeric(Request.QueryString("id"))) Then
                    Dim idPhoto As Integer = CInt(Request.QueryString("id"))
                    Dim isThumbnail As Boolean = False

                    If (Not Request.QueryString("thum") Is Nothing) Then
                        isThumbnail = True
                        If (IsNumeric(Request.QueryString("thum"))) Then idPhoto = CInt(Request.QueryString("id"))
                    End If

                    Dim portalDC As New DataSenalesCaribe.ContentDataContext(Comun.GetConnString)
                    Dim queryPhoto = From p In portalDC.Photos _
                                     Where p.IDPhoto = idPhoto _
                                     Select p
                    If (queryPhoto.Count > 0) Then

                        Response.Cache.SetExpires(DateTime.Now.AddHours(1))
                        Response.Cache.SetCacheability(HttpCacheability.Public)
                        Response.Cache.SetSlidingExpiration(True)
                        Response.ContentType = queryPhoto.First().ContentType

                        If (isThumbnail) Then
                            Response.BinaryWrite(queryPhoto.First().PhotoSmall.ToArray())
                        Else
                            Response.BinaryWrite(queryPhoto.First().PhotoLarge.ToArray())
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()
        Me.DoImageRender()
        Response.End()
    End Sub



End Class