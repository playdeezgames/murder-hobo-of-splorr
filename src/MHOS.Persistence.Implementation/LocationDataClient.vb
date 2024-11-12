Imports MHOS.Data

Public MustInherit Class LocationDataClient
    Inherits WorldDataClient
    Protected ReadOnly Property LocationId As Integer
    Protected ReadOnly Property LocationData As LocationData
        Get
            Return WorldData.Locations(LocationId)
        End Get
    End Property
    Public Sub New(worldData As Data.WorldData, locationId As Integer)
        MyBase.New(worldData)
        Me.LocationId = locationId
    End Sub
End Class
