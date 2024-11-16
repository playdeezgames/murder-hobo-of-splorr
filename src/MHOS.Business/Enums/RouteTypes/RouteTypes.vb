Friend Module RouteTypes
    Friend ReadOnly Road As String = NameOf(Road)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, RouteTypeDescriptor) =
        New List(Of RouteTypeDescriptor) From
        {
            New RoadRouteTypeDescriptor()
        }.ToDictionary(Function(x) x.RouteType, Function(x) x)
End Module
