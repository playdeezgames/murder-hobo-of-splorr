Friend Module AttributeTypes
    Friend ReadOnly Strength As String = NameOf(Strength)
    Friend ReadOnly Intelligence As String = NameOf(Intelligence)
    Friend ReadOnly Wisdom As String = NameOf(Wisdom)
    Friend ReadOnly Dexterity As String = NameOf(Dexterity)
    Friend ReadOnly Constitution As String = NameOf(Constitution)
    Friend ReadOnly Charisma As String = NameOf(Charisma)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, AttributeTypeDescriptor) =
        New List(Of AttributeTypeDescriptor) From
        {
            New StrengthAttributeTypeDescriptor(),
            New IntelligenceAttributeTypeDescriptor(),
            New WisdomAttributeTypeDescriptor(),
            New DexterityAttributeTypeDescriptor(),
            New ConstitutionAttributeTypeDescriptor(),
            New CharismaAttributeTypeDescriptor()
        }.ToDictionary(Function(x) x.AttributeType, Function(x) x)
End Module
