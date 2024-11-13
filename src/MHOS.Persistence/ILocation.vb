Public Interface ILocation
    Property LocationType As String
    ReadOnly Property Id As Integer
    Sub AddCharacter(result As ICharacter)
    Sub RemoveCharacter(character As ICharacter)
    ReadOnly Property HasCharacter As Boolean
    Function CreateRoute(direction As String, routeType As String, destination As ILocation) As IRoute
    Function HasRoute(direction As String) As Boolean
    Function GetRoute(direction As String) As IRoute
End Interface
