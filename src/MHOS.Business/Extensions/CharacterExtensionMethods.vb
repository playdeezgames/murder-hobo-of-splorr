Imports System.Diagnostics.CodeAnalysis

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
    <Extension>
    Friend Function Strength(character As ICharacter) As Integer
        Return character.Counter(CounterTypes.Strength).Value
    End Function
    <Extension>
    Friend Function Intelligence(character As ICharacter) As Integer
        Return character.Counter(CounterTypes.Intelligence).Value
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
    <Extension>
    Friend Function ExperiencePoints(character As ICharacter) As Integer
        Return character.Counter(CounterTypes.ExperiencePoints).Value
    End Function
    <Extension>
    Friend Function ExperienceLevel(character As ICharacter) As Integer
        Dim [class] = character.Metadata(MetadataTypes.Class)
        Dim xp = character.ExperiencePoints
        Dim classDescriptor = Classes.Descriptors([class])
        Dim levelDescriptors = classDescriptor.ClassLevelDescriptors.Where(Function(x) xp >= x.Value.ExperiencePoints)
        Return levelDescriptors.Max(Function(x) x.Key)
    End Function
    <Extension>
    Sub RollHitDice(character As ICharacter)
        Dim classDescriptor = character.GetClassDescriptor
        Dim raceDescriptor = character.GetRaceDescriptor
        Dim hitDie = Math.Min(classDescriptor.HitDie, raceDescriptor.MaximumHitDie)
        For Each classLevelDescriptor In classDescriptor.ClassLevelDescriptors
            Dim counterType = CounterTypes.LevelHitDieRoll(classLevelDescriptor.Key)
            character.Counter(counterType) = RNG.RollDice($"{classLevelDescriptor.Value.HitDice}d{hitDie}") + classLevelDescriptor.Value.HitPoints
        Next
    End Sub
    <Extension>
    Private Function GetRaceDescriptor(character As ICharacter) As BaseRaceDescriptor
        Return Races.Descriptors(character.Metadata(MetadataTypes.Race))
    End Function

    <Extension>
    Private Function GetClassDescriptor(character As ICharacter) As BaseClassDescriptor
        Return Classes.Descriptors(character.Metadata(MetadataTypes.Class))
    End Function
    <Extension>
    Friend Function MaximumHitPoints(character As ICharacter) As Integer
        Dim [class] = character.Metadata(MetadataTypes.Class)
        Dim xp = character.ExperiencePoints
        Dim classDescriptor = Classes.Descriptors([class])
        Dim levelDescriptors = classDescriptor.ClassLevelDescriptors.Where(Function(x) xp >= x.Value.ExperiencePoints)
        Return levelDescriptors.Sum(Function(x) Math.Max(1, character.Counter(x.Value.HitDieRollCounterType).Value + If(x.Value.HasConstitutionBonus, character.ConstitutionBonus, 0)))
    End Function
    <Extension>
    Friend Function HitPoints(character As ICharacter) As Integer
        Return Math.Clamp(character.Counter(CounterTypes.HitPoints).Value, 0, character.MaximumHitPoints)
    End Function
    <Extension>
    Friend Function ConstitutionBonus(character As ICharacter) As Integer
        Dim constitution = character.Counter(CounterTypes.Constitution)
        If Not constitution.HasValue Then
            Return 0
        End If
        Return attributeBonuses(constitution.Value)
    End Function
    <Extension>
    Friend Function Wisdom(character As ICharacter) As Integer
        Return character.Counter(CounterTypes.Wisdom).Value
    End Function
    Private ReadOnly attributeBonuses As IReadOnlyDictionary(Of Integer, Integer) =
        New Dictionary(Of Integer, Integer) From
        {
            {3, -3},
            {4, -2},
            {5, -2},
            {6, -1},
            {7, -1},
            {8, -1},
            {9, 0},
            {10, 0},
            {11, 0},
            {12, 0},
            {13, 1},
            {14, 1},
            {15, 1},
            {16, 2},
            {17, 2},
            {18, 3}
        }
End Module
