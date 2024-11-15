Friend Class ElfChoiceDescriptor
    Inherits BaseRaceChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Elf, "Elf")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Race) = Races.Elf
        Return Dialogs.Neutral
    End Function
End Class
