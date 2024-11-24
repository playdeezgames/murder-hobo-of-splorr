Friend Module CharacterExtensionMethods
    <Extension>
    Sub Initialize(character As ICharacter)
        character.Descriptor.Initialize(character)
    End Sub
    <Extension>
    Function Race(character As ICharacter) As String
        Return character.Metadata(MetadataTypes.Race)
    End Function
    <Extension>
    Function Descriptor(character As ICharacter) As BaseCharacterTypeDescriptor
        Return CharacterTypes.Descriptors(character.EntityType)
    End Function
    <Extension>
    Function DescribeAttributes(character As ICharacter) As IEnumerable(Of (Text As String, Mood As String))
        Return {
            ($"Strength {character.Strength}", Moods.Normal),
            ($"Intelligence {character.Intelligence}", Moods.Normal),
            ($"Wisdom {character.Wisdom}", Moods.Normal),
            ($"Dexterity {character.Dexterity}", Moods.Normal),
            ($"Constitution {character.Constitution}", Moods.Normal),
            ($"Charisma {character.Charisma}", Moods.Normal)
        }
    End Function
    <Extension>
    Private Function DescribeAttribute(character As ICharacter, counterType As String) As (Text As String, Mood As String)
        Return ($"{CounterTypes.Descriptors(counterType).Name} {character.Counter(counterType).Value}", Moods.Normal)
    End Function
    <Extension>
    Friend Function RaceName(character As ICharacter) As String
        Return Races.Descriptors(character.Race).Name
    End Function
    <Extension>
    Friend Function ClassName(character As ICharacter) As String
        Return Classes.Descriptors(character.Metadata(MetadataTypes.Class)).Name
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
        Dim classDescriptor = character.ClassDescriptor
        Dim raceDescriptor = character.RaceDescriptor
        Dim hitDie = Math.Min(classDescriptor.HitDie, raceDescriptor.MaximumHitDie)
        For Each classLevelDescriptor In classDescriptor.ClassLevelDescriptors
            character.LevelHitDieRoll(classLevelDescriptor.Key) = RNG.RollDice($"{classLevelDescriptor.Value.HitDice}d{hitDie}") + classLevelDescriptor.Value.HitPoints
        Next
    End Sub
    <Extension>
    Private Function RaceDescriptor(character As ICharacter) As BaseRaceDescriptor
        Return Races.Descriptors(character.Race)
    End Function

    <Extension>
    Private Function ClassDescriptor(character As ICharacter) As BaseClassDescriptor
        Return Classes.Descriptors(character.Metadata(MetadataTypes.Class))
    End Function
    <Extension>
    Friend Function MaximumHitPoints(character As ICharacter) As Integer
        Dim [class] = character.Metadata(MetadataTypes.Class)
        Dim xp = character.ExperiencePoints
        Dim classDescriptor = Classes.Descriptors([class])
        Dim levelDescriptors = classDescriptor.ClassLevelDescriptors.Where(Function(x) xp >= x.Value.ExperiencePoints)
        Return levelDescriptors.Sum(Function(x) Math.Max(1, character.LevelHitDieRoll(x.Key) + If(x.Value.HasConstitutionBonus, character.ConstitutionBonus, 0)))
    End Function
    <Extension>
    Friend Function HitPoints(character As ICharacter) As Integer
        Return Math.Clamp(character.HitPoints, 0, character.MaximumHitPoints)
    End Function
    <Extension>
    Friend Function ConstitutionBonus(character As ICharacter) As Integer
        Return attributeBonuses(character.Constitution)
    End Function
    <Extension>
    Friend Function AttackBonus(character As ICharacter) As Integer
        Return character.ClassDescriptor.ClassLevelDescriptors(character.ExperienceLevel).AttackBonus
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
