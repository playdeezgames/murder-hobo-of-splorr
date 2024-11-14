Imports MHOS.Data

Friend Class Location
    Inherits Entity(Of LocationData, Integer)
    Implements ILocation

    Public Sub New(worldData As Data.WorldData, locationId As Integer)
        MyBase.New(worldData, locationId)
    End Sub

    Public ReadOnly Property HasCharacter As Boolean Implements ILocation.HasCharacter
        Get
            Return EntityData.Characters.Any
        End Get
    End Property

    Protected Overrides ReadOnly Property EntityData As LocationData
        Get
            Return WorldData.Locations(EntityId)
        End Get
    End Property

    Public Sub AddCharacter(character As ICharacter) Implements ILocation.AddCharacter
        EntityData.Characters.Add(character.Id)
    End Sub

    Public Sub RemoveCharacter(character As ICharacter) Implements ILocation.RemoveCharacter
        EntityData.Characters.Remove(character.Id)
    End Sub

    Public Function CreateRoute(direction As String, routeType As String, destination As ILocation) As IRoute Implements ILocation.CreateRoute
        EntityData.Routes.Add(
            direction,
            New Data.RouteData With
            {
                .EntityType = routeType,
                .DestinationLocationId = destination.Id
            })
        Return New Route(WorldData, EntityId, direction)
    End Function

    Public Function HasRoute(direction As String) As Boolean Implements ILocation.HasRoute
        Return EntityData.Routes.ContainsKey(direction)
    End Function

    Public Function GetRoute(direction As String) As IRoute Implements ILocation.GetRoute
        If Not HasRoute(direction) Then
            Return Nothing
        End If
        Return New Route(WorldData, EntityId, direction)
    End Function
End Class
