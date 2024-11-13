Friend MustInherit Class BaseCounterTypeDescriptor
    ReadOnly Property AttributeType As String
    ReadOnly Property Name As String
    Sub New(counterType As String, name As String)
        Me.AttributeType = counterType
        Me.Name = name
    End Sub
End Class
