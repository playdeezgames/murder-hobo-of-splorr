
Friend Class Route
    Inherits RouteDataClient
    Implements IRoute

    Public Sub New(
                  worldData As Data.WorldData,
                  locationId As Integer,
                  direction As String)
        MyBase.New(
            worldData,
            locationId,
            direction)
    End Sub
End Class
