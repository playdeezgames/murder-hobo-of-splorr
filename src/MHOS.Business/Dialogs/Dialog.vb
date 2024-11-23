Public MustInherit Class Dialog
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
            Return Dialogs.Descriptors(Dialog).GoBackDialog(World) Is Nothing
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

    Public MustOverride ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IDialog.Description
    Public MustOverride ReadOnly Property CanEnterGameMenu As Boolean Implements IDialog.CanEnterGameMenu
    Public MustOverride ReadOnly Property AvailableChoices As IChoice() Implements IDialog.AvailableChoices
    Private ReadOnly Property Dialog As String Implements IDialog.Dialog

    Public Function LegacyGoBack() As IDialog Implements IDialog.LegacyGoBack
        Return Dialogs.Descriptors(Dialog).GoBackDialog(World)
    End Function

    Public MustOverride Function GoBack() As IDialog Implements IDialog.GoBack
End Class
