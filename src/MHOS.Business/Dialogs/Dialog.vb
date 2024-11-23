Public Class Dialog
    Implements IDialog
    Sub New(dialog As String, world As IWorld)
        Me.Dialog = dialog
        Me.World = world
    End Sub

    Public Property World As IWorld Implements IDialog.World

    Public ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IDialog.Description
        Get
            Return Dialogs.Descriptors(Dialog).Description(World)
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IDialog.CanEnterGameMenu
        Get
            Return String.IsNullOrEmpty(Dialogs.Descriptors(Dialog).GoBackDialog(World).Dialog)
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As IChoice() Implements IDialog.AvailableChoices
        Get
            Return Dialogs.
                Descriptors(Dialog).
                AvailableChoices(World).
                ToArray
        End Get
    End Property

    Private ReadOnly Property Dialog As String Implements IDialog.Dialog

    Public Function GoBack() As IDialog Implements IDialog.GoBack
        Return Dialogs.Descriptors(Dialog).GoBackDialog(World)
    End Function
End Class
