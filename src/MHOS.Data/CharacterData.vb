Public Class CharacterData
    Public Property LocationId As Integer
    Public Property Facing As String
    Public Property Attributes As New Dictionary(Of String, Integer)
    Public Property CharacterType As String
    Public Property Messages As New List(Of MessageData)
End Class
