Friend Class VerbChoice
    Inherits Choice
    Private ReadOnly verb As IVerb

    Public Sub New(world As IWorld, verb As IVerb)
        MyBase.New(world)
        Me.verb = verb
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return verb.Metadata(MetadataTypes.ShortName)
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return New NeutralDialog(world)
    End Function
End Class
