Friend Module Dialogs
    Friend ReadOnly Neutral As String = NameOf(Neutral)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly ChooseRace As String = NameOf(ChooseRace)
    Friend ReadOnly MoveMenu As String = NameOf(MoveMenu)
    Friend ReadOnly ChooseClass As String = NameOf(ChooseClass)
    Friend ReadOnly Initialize As String = NameOf(Initialize)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseDialogDescriptor) =
        New List(Of BaseDialogDescriptor) From
        {
            New NeutralDialogDescriptor(),
            New StatusDialogDescriptor(),
            New ChooseRaceDialogDescriptor(),
            New MoveMenuDialogDescriptor(),
            New ChooseClassDialogDescriptor(),
            New InitializeDialogDescriptor()
        }.ToDictionary(Function(x) x.Dialog, Function(x) x)
End Module
