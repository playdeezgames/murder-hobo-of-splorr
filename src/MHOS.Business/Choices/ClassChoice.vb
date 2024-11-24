Friend Class ClassChoice
    Inherits Choice
    Private ReadOnly [class] As String
    Public Sub New([class] As String, world As IWorld)
        MyBase.New(world)
        Me.class = [class]
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return Classes.Descriptors([class]).Name
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        With world.Avatar
            .Class = [class]
            .ExperiencePoints = 0
            .RollHitDice()
            .HitPoints = .MaximumHitPoints
        End With
        Return New NeutralDialog(world)
    End Function
End Class
