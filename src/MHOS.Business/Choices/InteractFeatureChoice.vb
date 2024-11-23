Friend Class InteractFeatureChoice
    Inherits Choice
    Private ReadOnly feature As IFeature

    Public Sub New(world As IWorld, feature As IFeature)
        MyBase.New(world)
        Me.feature = feature
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return feature.EntityType
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return New InteractFeatureDialog(world, feature)
    End Function
End Class
