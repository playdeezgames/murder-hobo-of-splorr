Friend Module Dialogs
    Friend ReadOnly Neutral As String = NameOf(Neutral)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly RollAttributes As String = NameOf(RollAttributes)
    Friend ReadOnly TurnMenu As String = NameOf(TurnMenu)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseDialogDescriptor) =
        New List(Of BaseDialogDescriptor) From
        {
            New NeutralDialogDescriptor(),
            New StatusDialogDescriptor(),
            New RollAttributesDialogDescriptor(),
            New TurnMenuDialogDescriptor()
        }.ToDictionary(Function(x) x.ChoiceMode, Function(x) x)
End Module
