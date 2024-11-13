Friend Class StatusChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Status, "Status")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Return Dialogs.Status
    End Function
End Class
