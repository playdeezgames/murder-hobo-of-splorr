Friend Module Choices
    Friend ReadOnly MoveAhead As String = NameOf(MoveAhead)
    Friend ReadOnly TurnLeft As String = NameOf(TurnLeft)
    Friend ReadOnly TurnRight As String = NameOf(TurnRight)
    Friend ReadOnly TurnAround As String = NameOf(TurnAround)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly Cancel As String = NameOf(Cancel)
    Friend ReadOnly Initialize As String = NameOf(Initialize)
    Friend ReadOnly [Next] As String = NameOf([Next])
    Friend ReadOnly TurnMenu As String = NameOf(TurnMenu)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseChoiceDescriptor) =
        New List(Of BaseChoiceDescriptor) From
        {
            New MoveAheadChoiceDescriptor(),
            New TurnLeftChoiceDescriptor(),
            New TurnRightChoiceDescriptor(),
            New TurnAroundChoiceDescriptor(),
            New StatusChoiceDescriptor(),
            New CancelChoiceDescriptor(),
            New InitializeChoiceDescriptor(),
            New NextChoiceDescriptor(),
            New TurnMenuChoiceDescriptor()
        }.ToDictionary(Function(x) x.Choice, Function(x) x)
End Module
