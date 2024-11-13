Friend Module LocationTypes
    Friend ReadOnly Initial As String = NameOf(Initial)
    Friend ReadOnly Room As String = NameOf(Room)
    Friend ReadOnly DeadEnd As String = NameOf(DeadEnd)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New List(Of LocationTypeDescriptor) From
        {
            New RoomLocationTypeDescriptor(),
            New InitialLocationTypeDescriptor(),
            New DeadEndLocationTypeDescriptor()
        }.ToDictionary(Function(x) x.LocationType, Function(x) x)
End Module
