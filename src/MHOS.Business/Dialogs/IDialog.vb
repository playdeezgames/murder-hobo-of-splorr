Public Interface IDialog
    ReadOnly Property Dialog As String
    Property World As IWorld
    Function LegacyGoBack() As IDialog
    ReadOnly Property LegacyDescription As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property LegacyCanEnterGameMenu As Boolean
    ReadOnly Property LegacyAvailableChoices As IChoice()
End Interface
