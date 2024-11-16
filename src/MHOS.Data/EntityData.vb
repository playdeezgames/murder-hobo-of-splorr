Public Class EntityData
    Public Property Counters As New Dictionary(Of String, Integer)
    Public Property Metadatas As New Dictionary(Of String, String)
    Public Property Flags As New HashSet(Of String)
    Public Property EntityType As String
End Class
