Friend Class ShoppeDialog
    Inherits Dialog

    Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            result.Add(($"Shoppe:", Moods.Heading))
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice)
            result.Add(CancelChoice.Create(Function() NeutralDialog.Create(World), World))
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return NeutralDialog.Create(World)
    End Function
End Class
