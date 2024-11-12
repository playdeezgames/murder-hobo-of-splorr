Friend Class AttributeModel
    Implements IAttributeModel
    Private ReadOnly Property Descriptor As AttributeTypeDescriptor
        Get
            Return AttributeTypes.Descriptors(attributeType)
        End Get
    End Property
    Public Sub New(attributeType As String, value As Integer)
        Me.attributeType = attributeType
        Me.Value = value
    End Sub
    Public ReadOnly Property Name As String Implements IAttributeModel.Name
        Get
            Return Descriptor.Name
        End Get
    End Property
    Public ReadOnly Property Value As Integer Implements IAttributeModel.Value

    Public ReadOnly Property AttributeType As String Implements IAttributeModel.AttributeType
End Class
