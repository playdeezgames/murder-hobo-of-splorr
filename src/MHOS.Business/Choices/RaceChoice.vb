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

    Private Function LegacyChoose() As String
        world.Avatar.Metadata(MetadataTypes.Race) = race
        Return Dialogs.ChooseClass
    End Function

    Public Overrides Function Choose() As IDialog
        Return New Dialog(LegacyChoose(), world)
    End Function
End Class
