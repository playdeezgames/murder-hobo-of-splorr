Friend Class RaceChoice
    Inherits Choice
    Private ReadOnly race As String

    Public Sub New(race As String, dialog As String, world As IWorld)
        MyBase.New(dialog, world)
        Me.race = race
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return Races.Descriptors(race).Name
        End Get
    End Property

    Public Overrides Function LegacyChoose() As String
        world.Avatar.Metadata(MetadataTypes.Race) = race
        Return Dialogs.ChooseClass
    End Function
End Class
