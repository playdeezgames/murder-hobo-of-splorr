Friend Class InteractFeatureDialog
    Inherits Dialog
    Private ReadOnly feature As IFeature

    Public Sub New(world As IWorld, feature As IFeature)
        MyBase.New(world)
        Me.feature = feature
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return {
                (feature.EntityType, Moods.Normal)
                }
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return {
                New CancelChoice(New InteractMenuDialog(World), World)
                }
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New InteractMenuDialog(World)
    End Function
End Class
