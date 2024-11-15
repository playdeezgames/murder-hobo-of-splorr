Friend Module Dialogs
    Friend ReadOnly Neutral As String = NameOf(Neutral)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly ChooseRace As String = NameOf(ChooseRace)
    Friend ReadOnly MoveMenu As String = NameOf(MoveMenu)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseDialogDescriptor) =
        New List(Of BaseDialogDescriptor) From
        {
            New NeutralDialogDescriptor(),
            New StatusDialogDescriptor(),
            New ChooseRaceDialogDescriptor(),
            New MoveMenuDialogDescriptor()
        }.ToDictionary(Function(x) x.ChoiceMode, Function(x) x)
End Module
