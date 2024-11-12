Friend Class LocationModel
    Implements ILocationModel
    ReadOnly location As ILocation
    Sub New(location As ILocation)
        Me.location = location
    End Sub
    Public ReadOnly Property Name As String Implements ILocationModel.Name
        Get
            Return location.Descriptor.Name
        End Get
    End Property
End Class
