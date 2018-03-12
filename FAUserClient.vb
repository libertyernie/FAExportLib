Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class FAUserClient
    Inherits FAClient

    Private ReadOnly _cookieA As String
    Private ReadOnly _cookieB As String

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

    Protected Overrides Function GetFACookie() As String
        Return $"b={_cookieB}; a={_cookieA}"
    End Function

    ''' <summary>
    ''' Posts a journal to FurAffinity.
    ''' </summary>
    ''' <param name="title">The title of the journal</param>
    ''' <param name="description">The body of the journal</param>
    ''' <returns>The URL of the created journal</returns>
    Public Async Function PostJournalAsync(title As String, description As String) As Task(Of String)
        Try
            Dim request = WebRequest.CreateHttp("https://faexport.boothale.net/journal.json")
            request.Method = "POST"
            request.UserAgent = UserAgent
            request.Headers.Add("FA_COOKIE", GetFACookie())
            Using sw As New StreamWriter(Await request.GetRequestStreamAsync)
                Dim body = New With {
                    .title = title,
                    .description = description
                }
                Await sw.WriteAsync(JsonConvert.SerializeObject(body))
            End Using
            Using response = Await request.GetResponseAsync
                Using sr As New StreamReader(response.GetResponseStream)
                    Dim json = Await sr.ReadToEndAsync
                    Dim dict = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(json)
                    Return dict("url")
                End Using
            End Using
        Catch ex As WebException
            Throw New FAExportException(ex)
        End Try
    End Function

    ''' <summary>
    ''' Get the logged in username (if any) by scraping it from the FurAffinity homepage.
    ''' </summary>
    ''' <returns>The username (without the leading tilde), or possibly null if the "a" and "b" cookies are not valid.</returns>
    Public Async Function WhoamiAsync() As Task(Of String)
        Dim url = "https://www.furaffinity.net"
        Dim request = WebRequest.CreateHttp(url)
        request.UserAgent = UserAgent
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
End Class
