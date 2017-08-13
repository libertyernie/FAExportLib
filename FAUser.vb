Public Class FAUser
    Public id As Integer?
    Public name As String
    Public profile As String
    Public account_type As String
    Public avatar As String
    Public full_name As String
    Public artist_type As String
    Public registered_since As String
    Public registered_at As Date
    Public current_mood As String
    Public artist_profile As String
    Public pageviews As Integer
    Public submissions As Integer
    Public comments_recieved As Integer
    Public comments_given As Integer
    Public journals As Integer
    Public favorites As Integer
    Public featured_submission As FASubmission
    Public profile_id As FASubmission
    Public artist_information As Dictionary(Of String, String)
    Public contact_information As IEnumerable(Of ContactInformation)
    Public watchers As WatchUserCollection
    Public watching As WatchUserCollection

    Public Class ContactInformation
        Public title As String
        Public name As String
        Public link As String
    End Class

    Public Class WatchUserCollection
        Public count As Integer
        Public recent As IEnumerable(Of WatchUser)
    End Class

    Public Class WatchUser
        Public name As String
        Public link As String
    End Class
End Class
