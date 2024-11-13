Friend Module ChoiceModes
    Friend ReadOnly Neutral As String = NameOf(Neutral)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ChoiceModeDescriptor) =
        New List(Of ChoiceModeDescriptor) From
        {
            New NeutralChoiceModeDescriptor(),
            New StatusChoiceModeDescriptor()
        }.ToDictionary(Function(x) x.ChoiceMode, Function(x) x)
End Module
