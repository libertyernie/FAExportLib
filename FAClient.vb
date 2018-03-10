Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAClient
    ''' <summary>
    ''' Create a client that connects to FAExport without logging in as a particular user.
    ''' This allows access to accounts and pictures hidden from guests, including mature and adult content, but does not allow posting journals.
    ''' </summary>
    Public Sub New()

    End Sub

    Public Property UserAgent As String = "FAClient/0.1 (https://github.com/libertyernie/FAClient)"

    Protected Overridable Function GetFACookie() As String
        Return Nothing
    End Function

    Private Async Function FAExportRequestAsync(url As String, Optional useCookie As Boolean = True) As Task(Of String)
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = UserAgent
        If useCookie Then
            Dim cookie = GetFACookie()
            If cookie IsNot Nothing Then
                request.Headers.Add("FA_COOKIE", cookie)
            End If
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
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?page={page}&perpage=60")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of Integer))(json)
    End Function

    ''' <summary>
    ''' Get a list of minimal submission data for a user's gallery, scraps, or favorites.
    ''' </summary>
    ''' <param name="username">A FurAffinity username</param>
    ''' <param name="folder">The folder type (gallery, scraps, or favorites)</param>
    ''' <param name="page">The page to start at (each page has up to 60 submissions)</param>
    Public Async Function GetSubmissionsAsync(username As String, folder As FAFolder, Optional page As Integer = 1) As Task(Of IEnumerable(Of FAFolderSubmission))
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/user/{WebUtility.UrlEncode(username)}/{folder.ToString("g")}.json?full=1&page={page}&perpage=60")
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of FAFolderSubmission))(json)
    End Function

    ''' <summary>
    ''' Get a list of submission IDs from a search query.
    ''' </summary>
    Public Async Function GetSearchIdsAsync(q As String,
                                            Optional page As Integer = 1,
                                            Optional order_by As FAOrder = FAOrder.date,
                                            Optional order_direction As FAOrderDirection = FAOrderDirection.desc,
                                            Optional range As FARange = FARange.all,
                                            Optional mode As FASearchMode = FASearchMode.extended,
                                            Optional rating As FARating = FARating.general Or FARating.mature Or FARating.adult,
                                            Optional type As FAType = FAType.art Or FAType.flash Or FAType.music Or FAType.photo Or FAType.poetry Or FAType.story) As Task(Of IEnumerable(Of Integer))
        Dim url = $"https://faexport.boothale.net/search.json?q={WebUtility.UrlEncode(q)}&page={page}&perpage=60&order_by={order_by}&order_direction={order_direction}&range={range}&mode={mode}&rating={rating.ToString().Replace(" ", "")}&type={type.ToString().Replace(" ", "")}"
        Dim json = Await FAExportRequestAsync(url)
        Return JsonConvert.DeserializeObject(Of IEnumerable(Of Integer))(json)
    End Function

    ''' <summary>
    ''' Get in-depth data about a particular submission.
    ''' </summary>
    ''' <param name="id">The numeric submission ID</param>
    Public Async Function GetSubmissionAsync(id As Integer) As Task(Of FASubmission)
        Dim json = Await FAExportRequestAsync($"https://faexport.boothale.net/submission/{id}.json", useCookie:=False)
        Return JsonConvert.DeserializeObject(Of FASubmission)(json)
    End Function
End Class
