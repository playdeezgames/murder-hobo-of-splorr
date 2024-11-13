Friend Class Character
    Inherits CharacterDataClient
    Implements ICharacter

    Public Sub New(worldData As Data.WorldData, characterId As Integer)
        MyBase.New(worldData, characterId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICharacter.Id
        Get
            Return CharacterId
        End Get
    End Property

    Public Property Location As ILocation Implements ICharacter.Location
        Get
            Return New Location(WorldData, CharacterData.LocationId)
        End Get
        Set(value As ILocation)
            If value.Id <> CharacterData.LocationId Then
                Location.RemoveCharacter(Me)
                CharacterData.LocationId = value.Id
                Location.AddCharacter(Me)
            End If
        End Set
    End Property

    Public Property Facing As String Implements ICharacter.Facing
        Get
            Return CharacterData.Facing
        End Get
        Set(value As String)
            CharacterData.Facing = value
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements ICharacter.World
        Get
            Return New World(WorldData)
        End Get
    End Property

    Public ReadOnly Property CounterTypes As IEnumerable(Of String) Implements ICharacter.CounterTypes
        Get
            Return CharacterData.Attributes.Keys
        End Get
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return CharacterData.CharacterType
        End Get
    End Property

    Public Property Counter(counterType As String) As Integer Implements ICharacter.Counter
        Get
            Return CharacterData.Attributes(counterType)
        End Get
        Set(value As Integer)
            CharacterData.Attributes(counterType) = value
        End Set
    End Property

    Public ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String)) Implements ICharacter.Messages
        Get
            Return CharacterData.Messages.Select(Function(x) (x.Text, x.Mood))
        End Get
    End Property

    Public Sub AddMessage(text As String, mood As String) Implements ICharacter.AddMessage
        CharacterData.Messages.Add(New Data.MessageData With {.Text = text, .Mood = mood})
    End Sub

    Public Sub ClearMessages() Implements ICharacter.ClearMessages
        CharacterData.Messages.Clear()
    End Sub
End Class