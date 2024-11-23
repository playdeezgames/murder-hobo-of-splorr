Public Interface IDialog
    ReadOnly Property Dialog As String
    Property World As IWorld
    Function LegacyGoBack() As IDialog
    Function GoBack() As IDialog
    ReadOnly Property LegacyDescription As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property LegacyCanEnterGameMenu As Boolean
    ReadOnly Property CanEnterGameMenu As Boolean
    ReadOnly Property LegacyAvailableChoices As IChoice()
    ReadOnly Property AvailableChoices As IChoice()
End Interface
