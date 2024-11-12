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
End Module
