Friend Module Directions
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly West As String = NameOf(West)
    Friend ReadOnly [In] As String = NameOf([In])
    Friend ReadOnly Out As String = NameOf(Out)
    Friend ReadOnly Down As String = NameOf(Down)
    Friend ReadOnly Up As String = NameOf(Up)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, DirectionDescriptor) =
        New List(Of DirectionDescriptor) From
        {
            New NorthDirectionDescriptor(),
            New EastDirectionDescriptor(),
            New SouthDirectionDescriptor(),
            New WestDirectionDescriptor(),
            New InDirectionDescriptor(),
            New OutDirectionDescriptor(),
            New DownDirectionDescriptor(),
            New UpDirectionDescriptor()
        }.ToDictionary(Function(x) x.Direction, Function(x) x)
End Module
