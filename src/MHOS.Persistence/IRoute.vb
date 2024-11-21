Public Interface IRoute
    Inherits IEntity(Of (LocationId As Integer, Direction As String))
    Property Destination As ILocation
End Interface
