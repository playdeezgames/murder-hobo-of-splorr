Friend Class StatusDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Status)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Cancel
            }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String)) From {
            ShowAttribute(world, AttributeTypes.Strength),
            ShowAttribute(world, AttributeTypes.Intelligence),
            ShowAttribute(world, AttributeTypes.Wisdom),
            ShowAttribute(world, AttributeTypes.Dexterity),
            ShowAttribute(world, AttributeTypes.Constitution),
            ShowAttribute(world, AttributeTypes.Charisma)
        }
        Return result
    End Function

    Private Function ShowAttribute(world As IWorld, attributeType As String) As (Text As String, Mood As String)
        Dim attributeDescriptor = AttributeTypes.Descriptors(attributeType)
        Dim avatar = world.Avatar
        Return ($"{attributeDescriptor.Name} {avatar.Attribute(attributeType)}", Moods.Normal)
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function
End Class
