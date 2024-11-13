Friend Module Directions
    Friend ReadOnly North As String = NameOf(North)
    Friend ReadOnly East As String = NameOf(East)
    Friend ReadOnly South As String = NameOf(South)
    Friend ReadOnly West As String = NameOf(West)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, DirectionDescriptor) =
        New List(Of DirectionDescriptor) From
        {
            New NorthDirectionDescriptor(),
            New EastDirectionDescriptor(),
            New SouthDirectionDescriptor(),
            New WestDirectionDescriptor()
        }.ToDictionary(Function(x) x.Direction, Function(x) x)
End Module
