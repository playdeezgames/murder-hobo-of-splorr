Public Interface ILocation
    Inherits IEntity(Of Integer)
    Sub AddCharacter(character As ICharacter)
    Sub RemoveCharacter(character As ICharacter)
    ReadOnly Property HasCharacter As Boolean
    Function CreateRoute(direction As String, routeType As String, destination As ILocation) As IRoute
    Function HasRoute(direction As String) As Boolean
    Function GetRoute(direction As String) As IRoute
    ReadOnly Property Routes As IEnumerable(Of IRoute)
    Function CreateFeature(featureType As String) As IFeature
    Function HasFeatures() As Boolean
    ReadOnly Property Features As IEnumerable(Of IFeature)
End Interface
