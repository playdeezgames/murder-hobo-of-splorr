Friend Module ConditionTypes
    Friend ReadOnly FlagRequired As String = NameOf(FlagRequired)
    Friend ReadOnly FlagForbidden As String = NameOf(FlagForbidden)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseConditionTypeDescriptor) =
        New List(Of BaseConditionTypeDescriptor) From
        {
            New FlagRequiredConditionTypeDescriptor(),
            New FlagForbiddenConditionTypeDescriptor()
        }.ToDictionary(Function(x) x.ConditionType, Function(x) x)
End Module
