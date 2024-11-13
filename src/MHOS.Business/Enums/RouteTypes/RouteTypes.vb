Friend Module RouteTypes
    Friend ReadOnly Door As String = NameOf(Door)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, RouteTypeDescriptor) =
        New List(Of RouteTypeDescriptor) From
        {
            New DoorRouteTypeDescriptor()
        }.ToDictionary(Function(x) x.RouteType, Function(x) x)
End Module
