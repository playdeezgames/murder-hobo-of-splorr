Friend Module CounterTypes
    Friend ReadOnly Strength As String = NameOf(Strength)
    Friend ReadOnly Intelligence As String = NameOf(Intelligence)
    Friend ReadOnly Wisdom As String = NameOf(Wisdom)
    Friend ReadOnly Dexterity As String = NameOf(Dexterity)
    Friend ReadOnly Constitution As String = NameOf(Constitution)
    Friend ReadOnly Charisma As String = NameOf(Charisma)
    Friend ReadOnly ExperiencePoints As String = NameOf(ExperiencePoints)
    Friend ReadOnly ExperienceLevel As String = NameOf(ExperienceLevel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor) =
        New List(Of BaseCounterTypeDescriptor) From
        {
            New StrengthCounterTypeDescriptor(),
            New IntelligenceCounterTypeDescriptor(),
            New WisdomCounterTypeDescriptor(),
            New DexterityCounterTypeDescriptor(),
            New ConstitutionCounterTypeDescriptor(),
            New CharismaCounterTypeDescriptor(),
            New ExperiencePointsCounterTypeDescriptor(),
            New ExperienceLevelCounterTypeDescriptor()
        }.ToDictionary(Function(x) x.AttributeType, Function(x) x)
End Module
