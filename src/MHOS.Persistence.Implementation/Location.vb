Friend Class Location
    Inherits LocationDataClient
    Implements ILocation

    Public Sub New(worldData As Data.WorldData, locationId As Integer)
        MyBase.New(worldData, locationId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ILocation.Id
        Get
            Return LocationId
        End Get
    End Property

    Public ReadOnly Property HasCharacter As Boolean Implements ILocation.HasCharacter
        Get
            Return LocationData.Characters.Any
        End Get
    End Property

    Public ReadOnly Property LocationType As String Implements ILocation.LocationType
        Get
            Return LocationData.LocationType
        End Get
    End Property

    Public Sub AddCharacter(character As ICharacter) Implements ILocation.AddCharacter
        LocationData.Characters.Add(character.Id)
    End Sub

    Public Sub RemoveCharacter(character As ICharacter) Implements ILocation.RemoveCharacter
        LocationData.Characters.Remove(character.Id)
    End Sub

    Public Function CreateRoute(direction As String, routeType As String, destination As ILocation) As IRoute Implements ILocation.CreateRoute
        LocationData.Routes.Add(
            direction,
            New Data.RouteData With
            {
                .RouteType = routeType,
                .DestinationLocationId = destination.Id
            })
        Return New Route(WorldData, LocationId, direction)
    End Function

    Public Function HasRoute(direction As String) As Boolean Implements ILocation.HasRoute
        Return LocationData.Routes.ContainsKey(direction)
    End Function

    Public Function GetRoute(direction As String) As IRoute Implements ILocation.GetRoute
        If Not HasRoute(direction) Then
            Return Nothing
        End If
        Return New Route(WorldData, LocationId, direction)
    End Function
End Class
