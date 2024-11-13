Friend Class TurnMenuChoiceModeDescriptor
    Inherits ChoiceModeDescriptor

    Public Sub New()
        MyBase.New(ChoiceModes.TurnMenu)
    End Sub

    Public Overrides Function GoBackChoiceMode(world As IWorld) As String
        Return ChoiceModes.Neutral
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Cancel,
            Choices.TurnRight,
            Choices.TurnLeft,
            Choices.TurnAround
            }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {
            ("Turn which way?", Moods.Normal)
            }
    End Function
End Class
