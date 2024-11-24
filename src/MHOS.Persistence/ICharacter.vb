Public Interface ICharacter
    Inherits IEntity(Of Integer)
    Property Location As ILocation
    Sub AddMessage(text As String, mood As String)
    Sub ClearMessages()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
    Property Strength As Integer
End Interface
