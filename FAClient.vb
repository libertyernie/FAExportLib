Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAClient
    Private Shared Async Function RequestAsync(url As String) As Task(Of String)
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = "FAClient/0.1 (https://github.com/libertyernie/FAClient"
        Using response = Await request.GetResponseAsync
            Using sr As New StreamReader(response.GetResponseStream)
                Return Await sr.ReadToEndAsync
            End Using
        End Using
    End Function

    Public Async Function GetUserAsync(name As String) As Task(Of FAUser)
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}.json")
        Return JsonConvert.DeserializeObject(Of FAUser)(json)
    End Function

    Public Async Function GetSubmissionIdsAsync(name As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of Integer))
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}/{folder.ToString("g")}.json")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of Integer))(json)
    End Function

    Public Async Function GetSubmissionsAsync(name As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of FASubmission))
        Dim json = Await RequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}/{folder.ToString("g")}.json?full=1")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of FASubmission))(json)
    End Function
End Class
