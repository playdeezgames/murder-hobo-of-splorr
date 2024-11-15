Friend Class ManChoiceDescriptor
    Inherits BaseRaceChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Man, "Man")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Race) = Races.Man
        Return Dialogs.Neutral
    End Function
End Class
