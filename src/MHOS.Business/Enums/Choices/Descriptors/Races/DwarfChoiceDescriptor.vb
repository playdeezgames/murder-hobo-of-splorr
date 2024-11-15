Friend Class DwarfChoiceDescriptor
    Inherits BaseRaceChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Dwarf, "Dwarf")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Race) = Races.Dwarf
        Return Dialogs.Neutral
    End Function
End Class
