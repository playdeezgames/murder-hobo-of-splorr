Friend MustInherit Class AttributeTypeDescriptor
    ReadOnly Property AttributeType As String
    ReadOnly Property Name As String
    Sub New(attributeType As String, name As String)
        Me.AttributeType = attributeType
        Me.Name = name
    End Sub
End Class
