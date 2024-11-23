Friend Module RouteExtensionMethods
    <Extension>
    Friend Function DirectionName(route As IRoute) As String
        Return Directions.Descriptors(route.Id.Direction).Name
    End Function
    <Extension>
    Friend Function RouteTypeName(route As IRoute) As String
        Return RouteTypes.Descriptors(route.EntityType).Name
    End Function
    <Extension>
    Friend Function Allows(route As IRoute, character As ICharacter) As Boolean
        Return route.Conditions.All(Function(x) x.Allows(character))
    End Function
End Module
