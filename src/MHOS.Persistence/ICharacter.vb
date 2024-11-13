Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
    Property Attribute(attributeType As String) As Integer
    ReadOnly Property Attributes As IEnumerable(Of String)
    ReadOnly Property CharacterType As String
    Sub AddMessage(text As String, mood As String)
    Sub ClearMessages()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
End Interface
