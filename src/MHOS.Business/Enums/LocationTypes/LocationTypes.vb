Friend Module LocationTypes
    Friend ReadOnly Town As String = NameOf(Town)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New List(Of LocationTypeDescriptor) From
        {
            New TownLocationTypeDescriptor()
        }.ToDictionary(Function(x) x.LocationType, Function(x) x)
End Module
