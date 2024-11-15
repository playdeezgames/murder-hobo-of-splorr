Friend Class BaseClassChoiceDescriptor
    Inherits BaseChoiceDescriptor
    ReadOnly Property [class] As String

    Public Sub New(choice As String, text As String, [class] As String)
        MyBase.New(choice, text)
        Me.class = [class]
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Class) = [class]
        Return Dialogs.Neutral
    End Function
End Class
