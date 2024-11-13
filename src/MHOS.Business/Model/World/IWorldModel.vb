Public Interface IWorldModel
    ReadOnly Property Session As IWorldSessionModel
    ReadOnly Property Avatar As IAvatarModel
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property CanEnterGameMenu As Boolean
    ReadOnly Property AvailableChoices As (Text As String, Choice As String)()
    Sub MakeChoice(choice As String)
End Interface
