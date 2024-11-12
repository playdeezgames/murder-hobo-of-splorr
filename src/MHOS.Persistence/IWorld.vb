Public Interface IWorld
    ReadOnly Property Serialized As String
    Function CreateLocation() As ILocation
    Function CreateCharacter(location As ILocation, facing As String) As ICharacter
    Sub SetAvatar(character As ICharacter)
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    ReadOnly Property Avatar As ICharacter
End Interface
