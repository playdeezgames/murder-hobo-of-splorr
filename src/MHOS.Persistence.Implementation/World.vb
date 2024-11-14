Imports System.Text.Json
Imports MHOS.Data

Public Class World
    Implements IWorld
    Protected ReadOnly WorldData As WorldData
    Sub New(worldData As WorldData)
        Me.WorldData = worldData
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
            If Not WorldData.AvatarId.HasValue Then
                Return Nothing
            End If
            Return New Character(WorldData, WorldData.AvatarId.Value)
        End Get
    End Property

    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.AvatarId = character.Id
    End Sub

    Public Function CreateLocation(locationType As String) As ILocation Implements IWorld.CreateLocation
        Dim locationId = WorldData.Locations.Count
        WorldData.Locations.Add(New LocationData With {.EntityType = locationType})
        Return New Location(WorldData, locationId)
    End Function

    Public Function CreateCharacter(characterType As String, location As ILocation) As ICharacter Implements IWorld.CreateCharacter
        Dim characterId = WorldData.Characters.Count
        WorldData.Characters.Add(New CharacterData With
                                {
                                    .EntityType = characterType,
                                    .LocationId = location.Id
                                })
        Dim result = New Character(WorldData, characterId)
        location.AddCharacter(result)
        Return result
    End Function
End Class
