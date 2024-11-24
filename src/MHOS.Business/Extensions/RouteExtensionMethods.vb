Friend Module RouteExtensionMethods
    <Extension>
    Friend Function DirectionName(route As IRoute) As String
        Return Directions.Descriptors(route.Id.Direction).Name
    End Function
    <Extension>
    Friend Function RouteTypeName(route As IRoute) As String
        Return RouteTypes.Descriptors(route.EntityType).Name
    End Function
End Module
