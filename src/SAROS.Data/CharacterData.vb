Public Class CharacterData
    Public Property LocationId As Integer
    Public Property Facing As String
    Public Property Sanity As Integer
    Public Property MaximumSanity As Integer
    Public Property TriggerLevels As New Dictionary(Of String, Integer)
    Public Property AwarenessLevels As New Dictionary(Of String, Integer)
    Public Property Items As New HashSet(Of Integer)
End Class
