Friend Module Choices
    Friend ReadOnly MoveAhead As String = NameOf(MoveAhead)
    Friend ReadOnly TurnLeft As String = NameOf(TurnLeft)
    Friend ReadOnly TurnRight As String = NameOf(TurnRight)
    Friend ReadOnly TurnAround As String = NameOf(TurnAround)
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly Cancel As String = NameOf(Cancel)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ChoiceDescriptor) =
        New List(Of ChoiceDescriptor) From
        {
            New MoveAheadChoiceDescriptor(),
            New TurnLeftChoiceDescriptor(),
            New TurnRightChoiceDescriptor(),
            New TurnAroundChoiceDescriptor(),
            New StatusChoiceDescriptor(),
            New CancelChoiceDescriptor()
        }.ToDictionary(Function(x) x.Choice, Function(x) x)
End Module
