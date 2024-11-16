Friend Module LocationTypes
    Friend ReadOnly Room As String = NameOf(Room)
    Friend ReadOnly DeadEnd As String = NameOf(DeadEnd)
    Friend ReadOnly Town As String = NameOf(Town)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New List(Of LocationTypeDescriptor) From
        {
            New RoomLocationTypeDescriptor(),
            New DeadEndLocationTypeDescriptor(),
            New TownLocationTypeDescriptor()
        }.ToDictionary(Function(x) x.LocationType, Function(x) x)
End Module
