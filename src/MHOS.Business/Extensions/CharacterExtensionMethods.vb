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
        Return New List(Of (Text As String, Mood As String)) From {
            DescribeAttribute(character, CounterTypes.Strength),
            DescribeAttribute(character, CounterTypes.Intelligence),
            DescribeAttribute(character, CounterTypes.Wisdom),
            DescribeAttribute(character, CounterTypes.Dexterity),
            DescribeAttribute(character, CounterTypes.Constitution),
            DescribeAttribute(character, CounterTypes.Charisma)
        }
    End Function
    Private Function DescribeAttribute(character As ICharacter, counterType As String) As (Text As String, Mood As String)
        Dim attributeDescriptor = CounterTypes.Descriptors(counterType)
        Return ($"{attributeDescriptor.Name} {character.Counter(counterType).Value}", Moods.Normal)
    End Function
    <Extension>
    Friend Function RaceName(character As ICharacter) As String
        Return Races.Descriptors(character.Metadata(MetadataTypes.Race)).Name
    End Function
    <Extension>
    Friend Function ClassName(character As ICharacter) As String
        Return Classes.Descriptors(character.Metadata(MetadataTypes.Class)).Name
    End Function
End Module
