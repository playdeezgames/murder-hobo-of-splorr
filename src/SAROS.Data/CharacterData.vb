Public Class CharacterData
    Public Property LocationId As Integer
    Public Property Facing As String
    Public Property Sanity As Integer
    Public Property MaximumSanity As Integer
    Public Property TriggerLevels As New Dictionary(Of String, Integer)
End Class
