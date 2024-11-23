Friend Class ClassChoice
    Inherits Choice
    Private ReadOnly [class] As String
    Public Sub New([class] As String, dialog As String, world As IWorld)
        MyBase.New(dialog, world)
        Me.class = [class]
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return Classes.Descriptors([class]).Name
        End Get
    End Property

    Private Function LegacyChoose() As String
        With world.Avatar
            .Metadata(MetadataTypes.Class) = [class]
            .Counter(CounterTypes.ExperiencePoints) = 0
            .RollHitDice()
            .Counter(CounterTypes.HitPoints) = .MaximumHitPoints
        End With
        Return Dialogs.Neutral
    End Function

    Public Overrides Function Choose() As IDialog
        Return New Dialog(LegacyChoose(), world)
    End Function
End Class
