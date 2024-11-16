Imports System.ComponentModel

Public Interface IRoute
    Inherits IEntity(Of (LocationId As Integer, Direction As String))
    ReadOnly Property Destination As ILocation
End Interface
