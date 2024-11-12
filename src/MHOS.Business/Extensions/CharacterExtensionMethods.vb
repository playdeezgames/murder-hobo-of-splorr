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
        Dim descriptor = CharacterTypes.Descriptors(character.CharacterType)
        For Each attribute In descriptor.Attributes
            character.SetAttribute(attribute, descriptor.GenerateAttribute(attribute))
        Next
    End Sub
End Module
