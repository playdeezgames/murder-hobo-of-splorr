Friend Module ChoiceModes
    Friend ReadOnly Navigation As String = NameOf(Navigation)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ChoiceModeDescriptor) =
        New List(Of ChoiceModeDescriptor) From
        {
            New NavigationChoiceModeDescriptor(),
            New StatusChoiceModeDescriptor()
        }.ToDictionary(Function(x) x.ChoiceMode, Function(x) x)
End Module
