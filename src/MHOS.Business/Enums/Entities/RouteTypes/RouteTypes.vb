Friend Module RouteTypes
    Friend ReadOnly Road As String = NameOf(Road)
    Friend ReadOnly Gate As String = NameOf(Gate)
    Friend ReadOnly Door As String = NameOf(Door)
    Friend ReadOnly Stairs As String = NameOf(Stairs)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, RouteTypeDescriptor) =
        New List(Of RouteTypeDescriptor) From
        {
            New RoadRouteTypeDescriptor(),
            New GateRouteTypeDescriptor(),
            New DoorRouteTypeDescriptor(),
            New StairsRouteTypeDescriptor()
        }.ToDictionary(Function(x) x.RouteType, Function(x) x)
End Module
