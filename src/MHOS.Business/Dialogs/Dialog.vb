Public Class Dialog
    Implements IDialog
    Sub New(dialog As String, world As IWorld)
        Me.Dialog = dialog
        Me.World = world
    End Sub

    Public Property World As IWorld Implements IDialog.World

    Public ReadOnly Property LegacyDescription As IEnumerable(Of (Text As String, Mood As String)) Implements IDialog.LegacyDescription
        Get
            Return Dialogs.Descriptors(Dialog).Description(World)
        End Get
    End Property

    Public ReadOnly Property LegacyCanEnterGameMenu As Boolean Implements IDialog.LegacyCanEnterGameMenu
        Get
            Return String.IsNullOrEmpty(Dialogs.Descriptors(Dialog).GoBackDialog(World).Dialog)
        End Get
    End Property

    Public ReadOnly Property LegacyAvailableChoices As IChoice() Implements IDialog.LegacyAvailableChoices
        Get
            Return Dialogs.
                Descriptors(Dialog).
                AvailableChoices(World).
                ToArray
        End Get
    End Property

    Private ReadOnly Property Dialog As String Implements IDialog.Dialog

    Public Function LegacyGoBack() As IDialog Implements IDialog.LegacyGoBack
        Return Dialogs.Descriptors(Dialog).GoBackDialog(World)
    End Function
End Class
