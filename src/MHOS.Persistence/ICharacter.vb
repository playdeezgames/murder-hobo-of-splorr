Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
    Property LegacyCounter(counterType As String) As Integer
    Property Counter(counterType As String) As Integer?
    ReadOnly Property CounterTypes As IEnumerable(Of String)
    ReadOnly Property CharacterType As String
    Sub AddMessage(text As String, mood As String)
    Sub ClearMessages()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
End Interface
