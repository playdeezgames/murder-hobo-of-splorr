Friend Class StatusChoiceModeDescriptor
    Inherits ChoiceModeDescriptor

    Public Sub New()
        MyBase.New(ChoiceModes.Status)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Cancel
            }
    End Function

    Public Overrides Function MakeChoice(world As IWorld, choice As String) As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Throw New NotImplementedException()
    End Function
End Class
