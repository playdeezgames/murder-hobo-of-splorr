Friend Class RollAttributesChoiceModeDescriptor
    Inherits ChoiceModeDescriptor

    Public Sub New()
        MyBase.New(ChoiceModes.RollAttributes)
    End Sub

    Public Overrides Function CanEnterGameMenu(world As IWorld) As Boolean
        Return False
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Next
            }
    End Function

    Public Overrides Function MakeChoice(world As IWorld, choice As String) As String
        Return Choices.Descriptors(choice).Choose(world, ChoiceMode)
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
End Class
