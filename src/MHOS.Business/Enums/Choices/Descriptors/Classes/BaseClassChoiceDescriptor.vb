Friend Class BaseClassChoiceDescriptor
    Inherits BaseChoiceDescriptor
    ReadOnly Property [class] As String

    Public Sub New(choice As String, text As String, [class] As String)
        MyBase.New(choice, text)
        Me.class = [class]
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        With world.Avatar
            .Metadata(MetadataTypes.Class) = [class]
            .Counter(CounterTypes.ExperienceLevel) = 1
            .Counter(CounterTypes.ExperiencePoints) = 0
        End With
        Return Dialogs.Neutral
    End Function
End Class
