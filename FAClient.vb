Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAClient
    Private ReadOnly _cookieA As String
    Private ReadOnly _cookieB As String

    ''' <summary>
    ''' Create a client that connects to FAExport without logging in as a particular user.
    ''' This allows access to accounts and pictures hidden from guests, including mature and adult content, but does not allow posting journals.
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Create a client that connects to FAExport as a certain user.
    ''' </summary>
    ''' <param name="a">FurAffinity's "a" cookie</param>
    ''' <param name="b">FurAffinity's "b" cookie</param>
    Public Sub New(a As String, b As String)
        If a Is Nothing Then
            Throw New ArgumentNullException(NameOf(a))
        End If
        If b Is Nothing Then
            Throw New ArgumentNullException(NameOf(b))
        End If
        _cookieA = a
        _cookieB = b
    End Sub

    ''' <summary>
    ''' Get the logged in username (if any) by scraping it from the FurAffinity homepage.
    ''' </summary>
    ''' <returns>The username (without the leading tilde), or possibly null if the "a" and "b" cookies are not valid.</returns>
    Public Async Function WhoamiAsync() As Task(Of String)
        Dim url = "https://www.furaffinity.net"
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = "FAClient/0.1 (https://github.com/libertyernie/FAClient)"
        If request.CookieContainer Is Nothing Then
            request.CookieContainer = New CookieContainer()
        End If
        If _cookieA IsNot Nothing And _cookieB IsNot Nothing Then
            request.CookieContainer.Add(New Uri(url), New Cookie("a", _cookieA))
            request.CookieContainer.Add(New Uri(url), New Cookie("b", _cookieB))
        End If
        Using response = Await request.GetResponseAsync
            Using sr As New StreamReader(response.GetResponseStream)
                Do
                    Dim line = Await sr.ReadLineAsync
                    If line Is Nothing Then
                        Exit Do
                    End If
                    If line.Contains("my-username") Then
                        line = line.Substring(line.IndexOf("~") + 1)
                        Dim endTagInd = line.IndexOf("<")
                        If endTagInd >= 0 Then
                            line = line.Substring(0, endTagInd)
                            Return line
                        End If
                    End If
                Loop
                Return Nothing
            End Using
        End Using
    End Function

    Private Async Function FAExportRequestAsync(url As String) As Task(Of String)
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = "FAClient/0.1 (https://github.com/libertyernie/FAClient)"
        If _cookieA IsNot Nothing And _cookieB IsNot Nothing Then
            Dim cookie = $"b={_cookieB}; a={_cookieA}"
            request.Headers.Add("FA_COOKIE", cookie)
        End If
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

    ''' <summary>
    ''' Get information about a user.
    ''' </summary>
    ''' <param name="name">A FurAffinity username</param>
    Public Async Function GetUserAsync(name As String) As Task(Of FAUser)
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(name)}.json")
        Return JsonConvert.DeserializeObject(Of FAUser)(json)
    End Function

    ''' <summary>
    ''' Get a list of submission IDs for a user's gallery, scraps, or favorites.
    ''' </summary>
    ''' <param name="username">A FurAffinity username</param>
    ''' <param name="folder">The folder type (gallery, scraps, or favorites)</param>
    ''' <param name="page">The page to start at (each page has up to 60 submissions)</param>
    Public Async Function GetSubmissionIdsAsync(username As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of Integer))
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?page={page}")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of Integer))(json)
    End Function

    ''' <summary>
    ''' Get a list of minimal submission data for a user's gallery, scraps, or favorites.
    ''' </summary>
    ''' <param name="username">A FurAffinity username</param>
    ''' <param name="folder">The folder type (gallery, scraps, or favorites)</param>
    ''' <param name="page">The page to start at (each page has up to 60 submissions)</param>
    Public Async Function GetSubmissionsAsync(username As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of FAFolderSubmission))
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?full=1&page={page}")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of FAFolderSubmission))(json)
    End Function

    ''' <summary>
    ''' Get in-depth data about a particular submission.
    ''' </summary>
    ''' <param name="id">The numeric submission ID</param>
    Public Async Function GetSubmissionAsync(id As Integer) As Task(Of FASubmission)
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/submission/{id}.json")
        Return JsonConvert.DeserializeObject(Of FASubmission)(json)
    End Function
End Class
