Public Class Message
    Public Sub New(messageTitle As String, messageLines() As String)
        Me.MessageTitle = messageTitle
        Me.MessageLines = messageLines
    End Sub

    Public ReadOnly Property MessageTitle As String
    Public ReadOnly Property MessageLines As String()
End Class
