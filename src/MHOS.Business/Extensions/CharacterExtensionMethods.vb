Friend Module CharacterExtensionMethods
    <Extension>
    Function AheadDirection(character As ICharacter) As String
        Return character.Facing
    End Function
    <Extension>
    Function LeftDirection(character As ICharacter) As String
        Return Directions.Descriptors(character.Facing).LeftDirection
    End Function
    <Extension>
    Function RightDirection(character As ICharacter) As String
        Return Directions.Descriptors(character.Facing).RightDirection
    End Function
    <Extension>
    Function OppositeDirection(character As ICharacter) As String
        Return Directions.Descriptors(character.Facing).OppositeDirection
    End Function
    <Extension>
    Sub Initialize(character As ICharacter)
        character.Descriptor.Initialize(character)
    End Sub
    <Extension>
    Function Descriptor(character As ICharacter) As BaseCharacterTypeDescriptor
        Return CharacterTypes.Descriptors(character.EntityType)
    End Function
End Module
