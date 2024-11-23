Friend Module ConditionTypes
    Friend ReadOnly FlagRequired As String = NameOf(FlagRequired)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseConditionTypeDescriptor) =
        New List(Of BaseConditionTypeDescriptor) From
        {
            New FlagRequiredConditionTypeDescriptor()
        }.ToDictionary(Function(x) x.ConditionType, Function(x) x)
End Module
