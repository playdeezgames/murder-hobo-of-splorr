Friend Module CounterTypes
    Friend ReadOnly Strength As String = NameOf(Strength)
    Friend ReadOnly Intelligence As String = NameOf(Intelligence)
    Friend ReadOnly Wisdom As String = NameOf(Wisdom)
    Friend ReadOnly Dexterity As String = NameOf(Dexterity)
    Friend ReadOnly Constitution As String = NameOf(Constitution)
    Friend ReadOnly Charisma As String = NameOf(Charisma)
    Friend ReadOnly ExperiencePoints As String = NameOf(ExperiencePoints)
    Private ReadOnly HitDieRoll As String = NameOf(HitDieRoll)
    Private Const FirstLevel As Integer = 1
    Private Const LevelCount As Integer = 20
    Friend ReadOnly Property LevelHitDieRoll(level As Integer) As String
        Get
            Return $"{HitDieRoll}{level}"
        End Get
    End Property

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor) =
        GenerateDescriptors()

    Private Function GenerateDescriptors() As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor)
        Dim result = New List(Of BaseCounterTypeDescriptor) From
        {
            New StrengthCounterTypeDescriptor(),
            New IntelligenceCounterTypeDescriptor(),
            New WisdomCounterTypeDescriptor(),
            New DexterityCounterTypeDescriptor(),
            New ConstitutionCounterTypeDescriptor(),
            New CharismaCounterTypeDescriptor(),
            New ExperiencePointsCounterTypeDescriptor()
        }
        For Each level In Enumerable.Range(FirstLevel, LevelCount)
            result.Add(New LevelHitDieRollCounterTypeDescriptor(level))
        Next
        Return result.ToDictionary(Function(x) x.AttributeType, Function(x) x)
    End Function
End Module
