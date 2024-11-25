Friend Class StatusDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            With World.Avatar
                Dim result As New List(Of (Text As String, Mood As String)) From {
            }
                result.AddRange(.DescribeAttributes)
                Return result
            End With
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return {
            New CancelChoice(New NeutralDialog(World), World)
            }
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New NeutralDialog(World)
    End Function
End Class
