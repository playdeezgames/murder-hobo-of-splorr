Imports System.Text.Json
Imports SAROS.Data

Public Class World
    Inherits WorldDataClient
    Implements IWorld
    Public Sub New(worldData As WorldData)
        MyBase.New(worldData)
    End Sub

    Public ReadOnly Property Serialized As String Implements IWorld.Serialized
        Get
            Return JsonSerializer.Serialize(WorldData)
        End Get
    End Property

    Public ReadOnly Property Locations As IEnumerable(Of ILocation) Implements IWorld.Locations
        Get
            Return Enumerable.Range(0, WorldData.Locations.Count).Select(Function(x) New Location(WorldData, x))
        End Get
    End Property

    Public ReadOnly Property Avatar As ICharacter Implements IWorld.Avatar
        Get
            Return New Character(WorldData, WorldData.AvatarId.Value)
        End Get
    End Property

    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.AvatarId = character.Id
    End Sub

    Public Function CreateLocation(column As Integer, row As Integer) As ILocation Implements IWorld.CreateLocation
        Dim locationId = WorldData.Locations.Count
        WorldData.Locations.Add(New LocationData With
                                {
                                    .Column = column,
                                    .Row = row
                                })
        Return New Location(WorldData, locationId)
    End Function

    Public Function CreateCharacter(location As ILocation, facing As String) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        WorldData.Characters.Add(New CharacterData With
                                {
                                    .LocationId = location.Id,
                                    .Facing = facing
                                })
        Dim result = New Character(WorldData, characterId)
        location.AddCharacter(result)
        Return result
    End Function
End Class
