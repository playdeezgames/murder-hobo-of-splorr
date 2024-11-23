Friend Class NeutralDialog
    Inherits Dialog
    Private subdialog As IDialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
        If world?.Avatar IsNot Nothing Then
            subdialog = New NavigationDialog(world)
        Else
            subdialog = New UninitializedDialog(world)
        End If
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return subdialog.Description
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return subdialog.AvailableChoices
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return subdialog.GoBack
    End Function
End Class
