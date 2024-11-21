Friend MustInherit Class BaseLocationTypeDescriptor
    ReadOnly Property LocationType As String
    ReadOnly Property Name As String
    Sub New(locationType As String, name As String)
        Me.LocationType = locationType
        Me.Name = name
    End Sub
End Class
