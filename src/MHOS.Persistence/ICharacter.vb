Public Interface ICharacter
    Inherits IEntity(Of Integer)
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
    Sub AddMessage(text As String, mood As String)
    Sub ClearMessages()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
End Interface
