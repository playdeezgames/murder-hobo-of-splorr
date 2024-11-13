Friend Class RouteDataClient
    Inherits LocationDataClient
    Protected ReadOnly Property Direction As String
    Protected ReadOnly Property RouteData As Data.RouteData
        Get
            Return LocationData.Routes(Direction)
        End Get
    End Property
    Public Sub New(worldData As Data.WorldData, locationId As Integer, direction As String)
        MyBase.New(worldData, locationId)
        Me.Direction = direction
    End Sub
End Class
