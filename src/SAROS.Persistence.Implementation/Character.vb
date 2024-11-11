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
                AddKnownLocation(value)
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

    Public Property Sanity As Integer Implements ICharacter.Sanity
        Get
            Return CharacterData.Sanity
        End Get
        Set(value As Integer)
            CharacterData.Sanity = Math.Clamp(value, 0, MaximumSanity)
        End Set
    End Property

    Public ReadOnly Property MaximumSanity As Integer Implements ICharacter.MaximumSanity
        Get
            Return CharacterData.MaximumSanity
        End Get
    End Property

    Public ReadOnly Property HasItems As Boolean Implements ICharacter.HasItems
        Get
            Return CharacterData.Items.Any
        End Get
    End Property

    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements ICharacter.Items
        Get
            Return CharacterData.Items.Select(Function(x) New Item(WorldData, x))
        End Get
    End Property

    Public ReadOnly Property Win As Boolean Implements ICharacter.Win
        Get
            Return CharacterData.TriggerLevels.All(Function(x) x.Value <= 0)
        End Get
    End Property

    Public ReadOnly Property World As IWorld Implements ICharacter.World
        Get
            Return New World(WorldData)
        End Get
    End Property

    Public Sub SetTriggerLevel(trauma As String, triggerLevel As Integer) Implements ICharacter.SetTriggerLevel
        CharacterData.TriggerLevels(trauma) = Math.Clamp(triggerLevel, 0, 24)
    End Sub

    Public Sub SetAwarenessLevel(trauma As String, awarenessLevel As Integer) Implements ICharacter.SetAwarenessLevel
        CharacterData.AwarenessLevels(trauma) = Math.Clamp(awarenessLevel, 0, 25)
    End Sub

    Public Sub AddItem(item As IItem) Implements ICharacter.AddItem
        CharacterData.Items.Add(item.Id)
    End Sub

    Public Sub RemoveItem(item As IItem) Implements ICharacter.RemoveItem
        CharacterData.Items.Remove(item.Id)
    End Sub

    Public Sub AddKnownLocation(location As ILocation) Implements ICharacter.AddKnownLocation
        CharacterData.KnownLocations.Add(location.Id)
    End Sub

    Public Function GetTriggerLevel(trauma As String) As Integer Implements ICharacter.GetTriggerLevel
        If String.IsNullOrEmpty(trauma) Then
            Return 0
        End If
        Return CharacterData.TriggerLevels(trauma)
    End Function

    Public Function GetAwarenessLevel(trauma As String) As Integer Implements ICharacter.GetAwarenessLevel
        Return CharacterData.AwarenessLevels(trauma)
    End Function

    Public Function KnowsLocation(location As ILocation) As Boolean Implements ICharacter.KnowsLocation
        Return CharacterData.KnownLocations.Contains(location.Id)
    End Function
End Class