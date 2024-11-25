Friend Module CharacterExtensionMethods
    <Extension>
    Sub Initialize(character As ICharacter)
        character.Descriptor.Initialize(character)
    End Sub
    <Extension>
    Function Descriptor(character As ICharacter) As BaseCharacterTypeDescriptor
        Return CharacterTypes.Descriptors(character.EntityType)
    End Function
    <Extension>
    Function DescribeAttributes(character As ICharacter) As IEnumerable(Of (Text As String, Mood As String))
        Return {
        }
    End Function
End Module
