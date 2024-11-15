Friend MustInherit Class BaseRaceChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Private ReadOnly race As String

    Protected Sub New(choice As String, text As String, race As String)
        MyBase.New(choice, text)
        Me.race = race
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.Avatar.Metadata(MetadataTypes.Race) = race
        Return Dialogs.ChooseClass
    End Function
End Class
