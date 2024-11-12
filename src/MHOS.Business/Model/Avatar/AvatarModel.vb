Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As IWorld

    Public Sub New(world As IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property Attributes As IEnumerable(Of IAttributeModel) Implements IAvatarModel.Attributes
        Get
            Return world.Avatar.Attributes.Select(Function(x) New AttributeModel(x, world.Avatar.GetAttribute(x)))
        End Get
    End Property
End Class
