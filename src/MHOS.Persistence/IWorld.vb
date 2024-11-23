Public Interface IWorld
    ReadOnly Property Serialized As String
    Function CreateLocation(locationType As String) As ILocation
    Function CreateCharacter(characterType As String, location As ILocation) As ICharacter
    Function CreateCondition(conditionType As String) As ICondition
    Sub SetAvatar(character As ICharacter)
    ReadOnly Property Locations As IEnumerable(Of ILocation)
    ReadOnly Property Avatar As ICharacter
    ReadOnly Property InitializationStepCount As Integer
    Sub AddInitializationStep(initializer As Action(Of IWorld))
    Sub DoNextStep()
End Interface
