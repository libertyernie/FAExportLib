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
