Friend Class RollAttributesDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.RollAttributes)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Next
            }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String)) From {
            ShowAttribute(world, CounterTypes.Strength),
            ShowAttribute(world, CounterTypes.Intelligence),
            ShowAttribute(world, CounterTypes.Wisdom),
            ShowAttribute(world, CounterTypes.Dexterity),
            ShowAttribute(world, CounterTypes.Constitution),
            ShowAttribute(world, CounterTypes.Charisma)
        }
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.RollAttributes
    End Function

    Private Function ShowAttribute(world As IWorld, attributeType As String) As (Text As String, Mood As String)
        Dim attributeDescriptor = CounterTypes.Descriptors(attributeType)
        Dim avatar = world.Avatar
        Return ($"{attributeDescriptor.Name} {avatar.Counter(attributeType)}", Moods.Normal)
    End Function
End Class
