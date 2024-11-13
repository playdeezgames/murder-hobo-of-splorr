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
            ShowAttribute(world, AttributeTypes.Strength),
            ShowAttribute(world, AttributeTypes.Intelligence),
            ShowAttribute(world, AttributeTypes.Wisdom),
            ShowAttribute(world, AttributeTypes.Dexterity),
            ShowAttribute(world, AttributeTypes.Constitution),
            ShowAttribute(world, AttributeTypes.Charisma)
        }
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.RollAttributes
    End Function

    Private Function ShowAttribute(world As IWorld, attributeType As String) As (Text As String, Mood As String)
        Dim attributeDescriptor = AttributeTypes.Descriptors(attributeType)
        Dim avatar = world.Avatar
        Return ($"{attributeDescriptor.Name} {avatar.Attribute(attributeType)}", Moods.Normal)
    End Function
End Class
