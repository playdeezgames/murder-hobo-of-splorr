Public Interface IWorld
    ReadOnly Property Serialized As String
    ReadOnly Property MurderCounter As Integer
    ReadOnly Property AttemptCounter As Integer
    ReadOnly Property SuccessRate As Integer?
    Sub AttemptMurder()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
End Interface
