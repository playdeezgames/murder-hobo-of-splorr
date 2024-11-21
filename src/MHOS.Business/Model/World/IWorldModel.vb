Public Interface IWorldModel
    ReadOnly Property Session As IWorldSessionModel
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property CanEnterGameMenu As Boolean
    Sub GoBack()
    ReadOnly Property AvailableChoices As IChoice()
    Sub LegacyMakeChoice(choice As String)
    Sub MakeChoice(choice As IChoice)
End Interface
