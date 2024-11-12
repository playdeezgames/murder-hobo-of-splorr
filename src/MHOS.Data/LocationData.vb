Public Class LocationData
    Property Column As Integer
    Property Row As Integer
    Property Neighbors As New Dictionary(Of String, Integer)
    Property Doors As New Dictionary(Of String, String)
    Property Characters As New HashSet(Of Integer)
End Class
