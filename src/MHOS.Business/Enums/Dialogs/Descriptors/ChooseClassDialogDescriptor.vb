Friend Class ChooseClassDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New(dialog As String)
        MyBase.New(dialog)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Throw New NotImplementedException()
    End Function
End Class
