Public Class LocationData
    Property Neighbors As New Dictionary(Of String, Integer)
    Property Doors As New Dictionary(Of String, String)
    Property Characters As New HashSet(Of Integer)
    Property LocationType As String
End Class
