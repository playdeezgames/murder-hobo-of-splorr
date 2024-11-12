Friend Module LocationTypes
    Friend ReadOnly Room As String = NameOf(Room)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New List(Of LocationTypeDescriptor) From
        {
            New RoomLocationTypeDescriptor()
        }.ToDictionary(Function(x) x.LocationType, Function(x) x)
End Module
