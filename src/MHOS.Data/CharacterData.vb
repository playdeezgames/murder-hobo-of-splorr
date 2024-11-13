Public Class CharacterData
    Inherits EntityData
    Public Property LocationId As Integer
    Public Property Facing As String
    Public Property Messages As New List(Of MessageData)
End Class
