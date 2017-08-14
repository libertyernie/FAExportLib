Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAExportException
    Inherits Exception

    Public ReadOnly StatusCode As HttpStatusCode?
    Public ReadOnly FAError As String
    Public ReadOnly FAUrl As String

    Public Sub New(ex As WebException)
        MyBase.New(ex.Message)
        StatusCode = TryCast(ex.Response, HttpWebResponse)?.StatusCode
        Try
            Using sr As New StreamReader(ex.Response.GetResponseStream)
                Dim json = sr.ReadToEnd
                Dim dict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(json)
                FAError = dict("error")
                FAUrl = dict("url")
            End Using
            ex.Response.Close()
        Catch ex2 As Exception

        End Try
    End Sub
End Class
