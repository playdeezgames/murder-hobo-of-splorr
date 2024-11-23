Imports System.Text.Json
Imports MHOS.Data

Public Class World
    Implements IWorld
    Protected ReadOnly WorldData As WorldData
    Private ReadOnly initializers As New Stack(Of Action(Of IWorld))
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
            Return Enumerable.Range(0, WorldData.Locations.Count).Where(Function(x) WorldData.Locations(x) IsNot Nothing).Select(Function(x) New Location(WorldData, x))
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

    Public ReadOnly Property InitializationStepCount As Integer Implements IWorld.InitializationStepCount
        Get
            Return initializers.Count
        End Get
    End Property

    Public Sub SetAvatar(character As ICharacter) Implements IWorld.SetAvatar
        WorldData.AvatarId = character.Id
    End Sub

    Public Sub AddInitializationStep(initializer As Action(Of IWorld)) Implements IWorld.AddInitializationStep
        initializers.Push(initializer)
    End Sub

    Public Sub DoNextStep() Implements IWorld.DoNextStep
        If initializers.Any Then
            Dim initializationStep = initializers.Pop()
            initializationStep.Invoke(Me)
        End If
    End Sub

    Public Function CreateLocation(locationType As String) As ILocation Implements IWorld.CreateLocation
        Dim locationId = WorldData.Locations.Count
        For Each candidateId In Enumerable.Range(0, WorldData.Locations.Count)
            If WorldData.Locations(candidateId) Is Nothing Then
                locationId = candidateId
                Exit For
            End If
        Next
        Dim locationData = New LocationData With {.EntityType = locationType}
        If locationId = WorldData.Locations.Count Then
            WorldData.Locations.Add(locationData)
        Else
            WorldData.Locations(locationId) = locationData
        End If
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

    Public Function CreateCondition(conditionType As String) As ICondition Implements IWorld.CreateCondition
        Dim conditionId = WorldData.Conditions.Count
        For Each candidateId In Enumerable.Range(0, WorldData.Conditions.Count)
            If WorldData.Conditions(candidateId) Is Nothing Then
                conditionId = candidateId
                Exit For
            End If
        Next
        Dim conditionData = New ConditionData With {.EntityType = conditionType}
        If conditionId = WorldData.Conditions.Count Then
            WorldData.Conditions.Add(conditionData)
        Else
            WorldData.Conditions(conditionId) = conditionData
        End If
        Return New Condition(WorldData, conditionId)
    End Function

    Public Function CreateVerb(verbType As String) As IVerb Implements IWorld.CreateVerb
        Dim verbId = WorldData.Verbs.Count
        For Each candidateId In Enumerable.Range(0, WorldData.Verbs.Count)
            If WorldData.Verbs(candidateId) Is Nothing Then
                verbId = candidateId
                Exit For
            End If
        Next
        Dim verbData = New VerbData With {.EntityType = verbType}
        If verbId = WorldData.Verbs.Count Then
            WorldData.Verbs.Add(verbData)
        Else
            WorldData.Verbs(verbId) = verbData
        End If
        Return New Verb(WorldData, verbId)
    End Function
End Class
