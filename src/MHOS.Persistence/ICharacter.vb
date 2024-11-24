Public Interface ICharacter
    Inherits IEntity(Of Integer)
    Property Location As ILocation
    Sub AddMessage(text As String, mood As String)
    Sub ClearMessages()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
    Property Strength As Integer
    Property Intelligence As Integer
    Property Wisdom As Integer
    Property Dexterity As Integer
End Interface
