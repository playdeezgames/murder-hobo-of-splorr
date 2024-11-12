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

    Public ReadOnly Property Attributes As IEnumerable(Of String) Implements ICharacter.Attributes
        Get
            Return CharacterData.Attributes.Keys
        End Get
    End Property

    Public ReadOnly Property CharacterType As String Implements ICharacter.CharacterType
        Get
            Return CharacterData.CharacterType
        End Get
    End Property

    Public Sub SetAttribute(attributeType As String, value As Integer) Implements ICharacter.SetAttribute
        CharacterData.Attributes(attributeType) = value
    End Sub

    Public Function GetAttribute(attributeType As String) As Integer Implements ICharacter.GetAttribute
        Return CharacterData.Attributes(attributeType)
    End Function
End Class