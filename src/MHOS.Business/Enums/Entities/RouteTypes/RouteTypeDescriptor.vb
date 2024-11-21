Friend MustInherit Class RouteTypeDescriptor
    ReadOnly Property RouteType As String
    ReadOnly Property Name As String
    Sub New(routeType As String, name As String)
        Me.RouteType = routeType
        Me.Name = name
    End Sub
End Class
