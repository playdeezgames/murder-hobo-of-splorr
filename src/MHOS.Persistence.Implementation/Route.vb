
Imports MHOS.Data

Friend Class Route
    Inherits Entity(Of RouteData, (LocationId As Integer, Direction As String))
    Implements IRoute

    Public Sub New(
                  worldData As Data.WorldData,
                  locationId As Integer,
                  direction As String)
        MyBase.New(
            worldData,
            (locationId,
            direction))
    End Sub

    Public ReadOnly Property Destination As ILocation Implements IRoute.Destination
        Get
            Return New Location(WorldData, EntityData.DestinationLocationId)
        End Get
    End Property

    Protected Overrides ReadOnly Property EntityData As RouteData
        Get
            Return WorldData.Locations(EntityId.LocationId).Routes(EntityId.Direction)
        End Get
    End Property
End Class
