Public Class FAFolderSubmission
    Public id As Integer
    Public title As String
    Public thumbnail As String
    Public link As String
End Class

Public Class FASubmission
    Public title As String
    Public description As String
    Public name As String
    Public profile As String
    Public link As String
    Public posted As String
    Public posted_at As DateTimeOffset
    Public download As String
    Public thumbnail As String
    Public category As String
    Public theme As String
    Public species As String
    Public gender As String
    Public favorites As Integer
    Public comments As Integer
    Public views As Integer
    Public resolution As String
    Public rating As String
    Public keywords As IEnumerable(Of String)
End Class
