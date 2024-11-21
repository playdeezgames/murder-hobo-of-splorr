Public Interface IWorldModel
    ReadOnly Property Session As IWorldSessionModel
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property CanEnterGameMenu As Boolean
    Sub GoBack()
    ReadOnly Property LegacyAvailableChoices As (Text As String, Choice As String)()
    ReadOnly Property AvailableChoices As IChoice()
    Sub MakeChoice(choice As String)
End Interface
