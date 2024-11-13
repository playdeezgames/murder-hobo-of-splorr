
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

    Public ReadOnly Property RouteType As String Implements IRoute.RouteType
        Get
            Return RouteData.RouteType
        End Get
    End Property

    Public ReadOnly Property Destination As ILocation Implements IRoute.Destination
        Get
            Return New Location(WorldData, RouteData.DestinationLocationId)
        End Get
    End Property
End Class
