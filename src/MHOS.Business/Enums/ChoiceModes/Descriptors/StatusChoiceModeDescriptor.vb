Friend Class StatusChoiceModeDescriptor
    Inherits ChoiceModeDescriptor

    Public Sub New()
        MyBase.New(ChoiceModes.Status)
    End Sub

    Public Overrides ReadOnly Property LegacyCanEnterGameMenu As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Cancel
            }
    End Function

    Public Overrides Function MakeChoice(world As IWorld, choice As String) As String
        Return ChoiceModes.Navigation
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {
            ("TODO: Status", Moods.Normal)
            }
    End Function

    Public Overrides Function CanEnterGameMenu(world As IWorld) As Boolean
        Return False
    End Function
End Class
