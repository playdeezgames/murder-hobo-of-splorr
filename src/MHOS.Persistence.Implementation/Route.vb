
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

    Public Property Destination As ILocation Implements IRoute.Destination
        Get
            Return New Location(WorldData, EntityData.DestinationLocationId)
        End Get
        Set(value As ILocation)
            EntityData.DestinationLocationId = value.Id
        End Set
    End Property

    Public ReadOnly Property Conditions As IEnumerable(Of ICondition) Implements IRoute.Conditions
        Get
            Return EntityData.Conditions.Select(Function(x) New Condition(WorldData, x))
        End Get
    End Property

    Protected Overrides ReadOnly Property EntityData As RouteData
        Get
            Return WorldData.Locations(EntityId.LocationId).Routes(EntityId.Direction)
        End Get
    End Property

    Public Overrides Sub Recycle()
        WorldData.Locations(EntityId.LocationId).Routes.Remove(EntityId.Direction)
    End Sub

    Public Function CreateCondition(conditionType As String) As ICondition Implements IRoute.CreateCondition
        Dim condition = World.CreateCondition(conditionType)
        EntityData.Conditions.Add(condition.Id)
        Return condition
    End Function
End Class
