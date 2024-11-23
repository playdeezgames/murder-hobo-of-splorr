Public Interface IRoute
    Inherits IEntity(Of (LocationId As Integer, Direction As String))
    Property Destination As ILocation
    Function CreateCondition(conditionType As String) As ICondition
    ReadOnly Property Conditions As IEnumerable(Of ICondition)
End Interface
