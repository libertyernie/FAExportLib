Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAClient
    Public Sub New()

    End Sub

    Private Async Function RequestAsync(url As String) As Task(Of String)
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = "FAClient/0.1 (https://github.com/libertyernie/FAClient)"
        Try
            Using response = Await request.GetResponseAsync
                Using sr As New StreamReader(response.GetResponseStream)
                    Return Await sr.ReadToEndAsync
                End Using
            End Using
        Catch ex As WebException
            Throw New FAExportException(ex)
        End Try
    End Function

    Public Async Function GetUserAsync(name As String) As Task(Of FAUser)
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}.json")
        Return JsonConvert.DeserializeObject(Of FAUser)(json)
    End Function

    Public Async Function GetSubmissionIdsAsync(username As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of Integer))
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?page={page}")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of Integer))(json)
    End Function

    Public Async Function GetSubmissionsAsync(username As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of FAFolderSubmission))
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?full=1&page={page}")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of FAFolderSubmission))(json)
    End Function

    Public Async Function GetSubmissionAsync(id As Integer) As Task(Of FASubmission)
        Dim json = Await RequestAsync($"https://faexport.boothale.net/submission/{id}.json")
        Return JsonConvert.DeserializeObject(Of FASubmission)(json)
    End Function
End Class
