Friend Module CharacterExtensionMethods
    <Extension>
    Function AheadDirection(character As ICharacter) As String
        Return character.Facing
    End Function
    <Extension>
    Function LeftDirection(character As ICharacter) As String
        Return Direction.GetLeft(character.Facing)
    End Function
    <Extension>
    Function RightDirection(character As ICharacter) As String
        Return Direction.GetRight(character.Facing)
    End Function
    <Extension>
    Function OppositeDirection(character As ICharacter) As String
        Return Direction.GetOpposite(character.Facing)
    End Function
    <Extension>
    Sub Initialize(character As ICharacter)
        Dim descriptor = character.Descriptor
        For Each attribute In descriptor.Attributes
            character.Attribute(attribute) = descriptor.GenerateAttribute(attribute)
        Next
    End Sub
    <Extension>
    Function Descriptor(character As ICharacter) As CharacterTypeDescriptor
        Return CharacterTypes.Descriptors(character.CharacterType)
    End Function
End Module
