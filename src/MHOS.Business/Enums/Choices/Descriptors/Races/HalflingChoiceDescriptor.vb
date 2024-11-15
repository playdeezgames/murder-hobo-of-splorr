Friend Class HalflingChoiceDescriptor
    Inherits BaseRaceChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Halfling, "Halfling")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Race) = Races.Halfling
        Return Dialogs.Neutral
    End Function
End Class
