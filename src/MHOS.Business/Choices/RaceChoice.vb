Friend Class RaceChoice
    Inherits Choice
    Private ReadOnly race As String

    Public Sub New(race As String, world As IWorld)
        MyBase.New(world)
        Me.race = race
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return Races.Descriptors(race).Name
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.Avatar.Metadata(MetadataTypes.Race) = race
        Return New ChooseClassDialog(world)
    End Function
End Class
