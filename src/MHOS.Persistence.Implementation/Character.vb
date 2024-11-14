Imports MHOS.Data

Friend Class Character
    Inherits Entity(Of CharacterData, Integer)
    Implements ICharacter
    Protected Overrides ReadOnly Property EntityData As CharacterData
        Get
            Return WorldData.Characters(EntityId)
        End Get
    End Property

    Public Sub New(worldData As Data.WorldData, characterId As Integer)
        MyBase.New(worldData, characterId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICharacter.Id
        Get
            Return EntityId
        End Get
    End Property

    Public Property Location As ILocation Implements ICharacter.Location
        Get
            Return New Location(worldData, EntityData.LocationId)
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.LocationId Then
                Location.RemoveCharacter(Me)
                EntityData.LocationId = value.Id
                Location.AddCharacter(Me)
            End If
        End Set
    End Property

    Public Property Facing As String Implements ICharacter.Facing
        Get
            Return EntityData.Facing
        End Get
        Set(value As String)
            EntityData.Facing = value
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements ICharacter.World
        Get
            Return New World(worldData)
        End Get
    End Property

    Public ReadOnly Property CounterTypes As IEnumerable(Of String) Implements ICharacter.CounterTypes
        Get
            Return EntityData.Counters.Keys
        End Get
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return EntityData.EntityType
        End Get
    End Property

    Public ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String)) Implements ICharacter.Messages
        Get
            Return EntityData.Messages.Select(Function(x) (x.Text, x.Mood))
        End Get
    End Property

    Public Property Counter(counterType As String) As Integer? Implements ICharacter.Counter
        Get
            Dim counterValue As Integer = 0
            If EntityData.Counters.TryGetValue(counterType, counterValue) Then
                Return counterValue
            End If
            Return Nothing
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                EntityData.Counters(counterType) = value.Value
            Else
                EntityData.Counters.Remove(counterType)
            End If
        End Set
    End Property

    Public Sub AddMessage(text As String, mood As String) Implements ICharacter.AddMessage
        EntityData.Messages.Add(New Data.MessageData With {.Text = text, .Mood = mood})
    End Sub

    Public Sub ClearMessages() Implements ICharacter.ClearMessages
        EntityData.Messages.Clear()
    End Sub
End Class