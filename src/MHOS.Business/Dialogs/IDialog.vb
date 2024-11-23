Public Interface IDialog
    ReadOnly Property Dialog As String
    Property World As IWorld
    Function GoBack() As IDialog
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property AvailableChoices As IChoice()
End Interface
