Public Class RouteData
    Inherits EntityData
    Public Property DestinationLocationId As Integer
    Public Property Conditions As New HashSet(Of Integer)
End Class
